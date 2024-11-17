using HebrewBooks.Models;
using HebrewBooksInWord;
using HebrewBooksInWord.Resources;
using Microsoft.Web.WebView2.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HebrewBooks
{
    /// <summary>
    /// Interaction logic for HebrewBooksViewer.xaml
    /// </summary>
    public partial class HebrewBooksViewer : UserControl
    {
        public HebrewBooksViewer()
        {
            InitializeComponent();
            BooksListView.ItemsSource = BookEntriesList.BookEntries;
            LoadRecentBooks();

            this.Focus();
            tabControl.SelectedIndex = 0;
            SearchBox.Focus();

            Globals.ThisAddIn.Application.DocumentBeforeClose += Application_DocumentBeforeClose;
        }

        void LoadRecentBooks()
        {
            var recentBooks = SettingsHandler.LoadSetting<Dictionary<string, List<string>>>("RecentBooks", new Dictionary<string, List<string>>());
            if (!recentBooks.ContainsKey(Globals.ThisAddIn.Application.ActiveDocument.FullName)) return;
            
            var docRecentBooks = recentBooks[Globals.ThisAddIn.Application.ActiveDocument.FullName];
            if (docRecentBooks != null)
            {
                foreach (var recentBook in docRecentBooks)
                {
                    BookEntry entry = BookEntriesList.BookEntries.FirstOrDefault(b => b.Title == recentBook);
                    openSelectedBook(entry);
                }
            }
        }

        private void Application_DocumentBeforeClose(Microsoft.Office.Interop.Word.Document Doc, ref bool Cancel)
        {
            var recentBooks = SettingsHandler.LoadSetting<Dictionary <string, List<string>>>("RecentBooks", new Dictionary<string, List<string>>());
            if (recentBooks.Count > 9) { recentBooks.Remove(recentBooks.First().Key); }

            if (!recentBooks.ContainsKey(Doc.FullName))
                recentBooks.Add(Doc.FullName, new List<string>());
            else
                recentBooks[Doc.FullName] = new List<string>();
           

            foreach (TabItem tabItem in tabControl.Items)
            {
                if (tabItem.Header.ToString() != "בחר ספר")
                    recentBooks[Doc.FullName].Add(tabItem.Header.ToString());
            }

            SettingsHandler.SaveSetting("RecentBooks", recentBooks);
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox) Search(textBox.Text);
        }

        void Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                BookEntriesList.SortList();
                BooksListView.ItemsSource = BookEntriesList.BookEntries;
            }
            else
            {
                // Combine with recent items, giving priority to frequently accessed ones
                BooksListView.ItemsSource = BookEntriesList.Search(searchTerm);
            }
        }


        private void BooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is BookEntry entry) openSelectedBook(entry);
            BooksListView.SelectedIndex = -1;
        }

        async void openSelectedBook(BookEntry entry)
        {
            var viewer = new BookViewer(entry.ID_Book);
            tabControl.Items.Add(new TabItem { Header = entry.Title, Content = viewer, IsSelected = true });
            entry.Popularity++;
            if (entry.Popularity > short.MaxValue)
                await BookEntriesList.NormalizePopularityScore();
            await BookEntriesList.SaveBookEntriesListAsync();
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

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.X)
            {
                List<TabItem> tabitems = tabControl.Items.Cast<TabItem>().ToList();
                foreach (TabItem tabItem in tabitems)
                {
                    if (tabItem.Header.ToString() == "בחר ספר") continue;
                    if (tabItem.Content is WebView2 webView) { webView.Dispose(); }
                    tabControl.Items.Remove(tabItem);
                }
                e.Handled = true;
            }

            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.W)
            {
                if (tabControl.SelectedItem is TabItem tabItem && tabItem.Header.ToString() != "בחר ספר")
                {
                    if (tabItem.Content is WebView2 webView) { webView.Dispose(); }
                    tabControl.Items.Remove(tabItem);
                }
                e.Handled = true; 
            }

            else if (e.Key == Key.Tab)
            {
                if (tabControl.SelectedIndex >= tabControl.Items.Count - 1)
                    tabControl.SelectedIndex = 0;
                else
                    tabControl.SelectedIndex++;
                e.Handled = true; // Mark the event as handled if needed
            }
        }

    }
}