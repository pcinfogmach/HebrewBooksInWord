using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace HebrewBooks.Resources
{
    public static class GetResource
    {
        public static Bitmap GetHebrewBooksIcon()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(appPath, "Resources", "HebrewBooksIcon.jpg");

            if (File.Exists(fullPath))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fullPath, UriKind.Absolute);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                // Convert BitmapImage to Bitmap
                return BitmapImageToBitmap(bitmapImage);
            }
            else
            {
                throw new FileNotFoundException("Image file not found", fullPath);
            }
        }

        private static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder(); // Use BMP encoder
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return new Bitmap(memoryStream);
            }
        }
    }
}
