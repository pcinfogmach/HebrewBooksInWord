using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace HebrewBooks.Models
{
    internal class BookViewer : WebView2
    {
        public BookViewer(string id) 
        {
            Load(id);
        }

        async void Load(string id)
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
                            //MessageBox.Show($"Error downloading the file: {ex.Message}");
                        }
                    }
                }

                Source = new Uri(downloadPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
