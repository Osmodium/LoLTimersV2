using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LoLTimers.Utilities
{
    public static class ImageUtilities
    {
        public static ImageSource ConvertImageToGrayScaleImage(BitmapImage bmpImage)
        {
            FormatConvertedBitmap grayBitmap = new();
            grayBitmap.BeginInit();
            grayBitmap.Source = bmpImage;
            grayBitmap.DestinationFormat = PixelFormats.Gray8;
            grayBitmap.EndInit();
            return grayBitmap;
        }
    }
}
