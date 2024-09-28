using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HebrewBooks
{
    public static class Downloader
    {
        public async static void Download(string fileName, string id)
        {
            string url = $"https://download.hebrewbooks.org/downloadhandler.ashx?req={53057}";
            fileName = $"{fileName}.pdf"; // You can change the extension if it's not a PDF
            string downloadFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
            if(!Directory.Exists(downloadFolder)) Directory.CreateDirectory(downloadFolder);
            string downloadPath = Path.Combine(downloadFolder, fileName);

            if (!File.Exists(downloadPath))
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        byte[] fileBytes = await client.GetByteArrayAsync(url);
                        await File.WriteAllBytesAsync(downloadPath, fileBytes);
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Network error: {ex.Message}");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"File system error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unexpected error: {ex.Message}");
                    }
                }
            }
        }
    }
}
