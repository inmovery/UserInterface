using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace UserInterfaceTest.Helpers
{
    public static class Converter
    {
        public static BitmapImage FromBytesToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                throw new ArgumentException();

            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
