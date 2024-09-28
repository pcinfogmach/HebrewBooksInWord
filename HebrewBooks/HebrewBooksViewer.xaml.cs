using HebrewBooks.Models;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HebrewBooks
{
    /// <summary>
    /// Interaction logic for HebrewBooksViewer.xaml
    /// </summary>
    public partial class HebrewBooksViewer : UserControl
    {
        BookEntriesList bookEntries;
        public HebrewBooksViewer()
        {
            InitializeComponent();
            LoadBooksList();

        }

        async void LoadBooksList()
        {
            await Task.Run(() =>  {  bookEntries = new BookEntriesList();   });
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
                    var results = bookEntries.Where(entry => entry.Title.StartsWith(textBox.Text));
                    if (!results.Any()) results = bookEntries.Where(entry => entry.Author.StartsWith(textBox.Text));
                    if (!results.Any()) results = bookEntries.Where(entry => entry.Tags.Contains(textBox.Text));
                    BooksListView.ItemsSource = results;
                }
            }
        }

        private void BooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is BookEntry entry)
            {
                tabControl.Items.Add(new TabItem { Header = entry.Title, Content = new BookViewer(entry.ID_Book), IsSelected = true });
            }

            BooksListView.SelectedIndex = -1;
        }

        private void X_Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                TabItem tabItemToRemove = DependencyHelper.FindParent<TabItem>(button);

                if (tabItemToRemove != null)
                {
                    if (tabItemToRemove.Content is WebView2 webView) { webView.Dispose(); }
                    tabControl.Items.Remove(tabItemToRemove);
                }
            }
        }
    }
}