using HebrewBooks;
using Microsoft.Web.WebView2.Wpf;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BrowserTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<BookEntry> bookEntries;
        public MainWindow()
        {
            InitializeComponent();
            LoadBooksList();
            
        }

        async void LoadBooksList()
        {
            bookEntries = await Task.Run(() =>
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = System.IO.Path.Combine(appPath, "Resources", "FullIndex.txt");
                var entries = new List<BookEntry>();

                using (var reader = new StreamReader(fullPath))
                {
                    while (!reader.EndOfStream)
                    {
                        var splitEntry = reader.ReadLine().Split(',');
                        entries.Add(new BookEntry(splitEntry[0], splitEntry[1], splitEntry[2], splitEntry[3], splitEntry[4], splitEntry[5], splitEntry[6], splitEntry[7], splitEntry[8], splitEntry[9], splitEntry[10]));
                    }
                }

                entries.OrderBy(e => e.Title);
                return entries;
            });

            BooksListView.ItemsSource = bookEntries;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                    BooksListView.ItemsSource = bookEntries;
                else
                {
                    var results = bookEntries.Where(e => e.Title.StartsWith(textBox.Text));
                    if (!results.Any()) results = bookEntries.Where(e => e.Author.StartsWith(textBox.Text));
                    if (!results.Any()) results = bookEntries.Where(e => e.Tags.Contains(textBox.Text));
                    BooksListView.ItemsSource = results;
                }
            } 
        }

        private async void BooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is BookEntry entry)
            {
                var webView = new WebView2();
                tabControl.Items.Add(new TabItem { Header = entry.Title, Content = webView, IsSelected = true });

                webView.Source = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "LoadingAnimation.html"));
                string url = $"https://download.hebrewbooks.org/downloadhandler.ashx?req={entry.ID_Book}";
                string fileName = $"{entry.ID_Book}.pdf"; // You can change the extension if it's not a PDF
                string downloadPath = Path.Combine(Path.GetTempPath(), fileName);

                if (!File.Exists(downloadPath))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            byte[] fileBytes = await client.GetByteArrayAsync(url);
                            await File.WriteAllBytesAsync(downloadPath, fileBytes);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error downloading the file: {ex.Message}");
                        }
                    }
                }

                webView.Source = new Uri(downloadPath);
            }
        }

        private void X_Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button) 
            {
                TabItem tabItemToRemove = FindParent<TabItem>(button);

                // If the TabItem is found, remove it from the TabControl
                if (tabItemToRemove != null)
                {
                    if (tabItemToRemove.Content is WebView2 webView) {webView.Dispose(); }
                    tabControl.Items.Remove(tabItemToRemove);
                }
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            // Get the parent object
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // Check if we've reached the end of the tree
            if (parentObject == null) return null;

            // Check if the parent is of the specified type
            if (parentObject is T parent)
                return parent;
            else
                // Recursively look up the tree
                return FindParent<T>(parentObject);
        }
    }
}