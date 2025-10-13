using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace WPFGameEngine.WPF.GE.Helpers
{
    public static class ImageHelper
    {
        public static Image LoadImageUsingUri(string uri, UriKind kind)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(uri, kind);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            Image image = new Image();
            image.Source = bitmap;
            return image;
        }

        public static Image LoadImageUsingSrc(ImageSource imageSource)
        {
            Image image = new Image();
            image.Source = imageSource;
            return image;
        }
    }
}
