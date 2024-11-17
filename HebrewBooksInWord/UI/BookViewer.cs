using HebrewBooksInWord;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace HebrewBooks.Models
{
    internal class BookViewer : WebView2
    {
        Microsoft.Office.Interop.Word.Document ParentDoc;
        bool isLoaded = false;
        public BookViewer(string id) 
        {
            SetCore();
            CoreWebView2InitializationCompleted += (s, e) => 
            {
                var com = this.CoreWebView2.GetComICoreWebView2();
                LoadBook(id); 
            };

            var doc = Globals.ThisAddIn.Application.ActiveDocument;
            Globals.ThisAddIn.Application.DocumentBeforeClose += Application_DocumentBeforeClose;
            Globals.ThisAddIn.Shutdown += (s, e) => { try { this.Dispose(); } catch { } };
        }

        private void Application_DocumentBeforeClose(Microsoft.Office.Interop.Word.Document Doc, ref bool Cancel)
        {
            if (Doc == ParentDoc) { try { this.Dispose(); } catch { } }
        }

        public async void SetCore()
        {
            if (isLoaded == false)
            {
                try
                {
                    string tempWebCacheDir = AppDomain.CurrentDomain.BaseDirectory;
                    var webView2Environment = await CoreWebView2Environment.CreateAsync(userDataFolder: tempWebCacheDir);
                    await this.EnsureCoreWebView2Async(webView2Environment);
                    this.AllowExternalDrop = false;
                    isLoaded = true;
                }
                catch (InvalidOperationException)
                {
                    Loaded += (sender, e) => { SetCore(); };
                }
                catch { }
            }
        }

        async void LoadBook(string id)
        {
            try
            {
                Source = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "LoadingAnimation.html"));
                string url = $"https://download.hebrewbooks.org/downloadhandler.ashx?req={id}";
                string fileName = $"{id}.pdf"; // You can change the extension if it's not a PDF
                string downloadPath = Path.Combine(Path.GetTempPath(), fileName);

                if (!File.Exists(downloadPath))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            byte[] fileBytes = await client.GetByteArrayAsync(url);
                            File.WriteAllBytes(downloadPath, fileBytes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error downloading the file: {ex.Message}");
                        }
                    }
                }

                //CoreWebView2.NavigationCompleted += (sender, e) => { this.ExecuteScriptAsync("document.querySelectorAll('*').forEach(function(element) {element.setAttribute(\"dir\", \"auto\");});"); };
                Source = new Uri(downloadPath);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
