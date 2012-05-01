using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Gi7.Client.Model;

namespace Gi7.Utils.Converters
{
    public class PathToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = value as GitHubFile;

            var imageSource = new Uri("/Images/folder.png", UriKind.RelativeOrAbsolute);
            if (obj.Type == "blob")
            {
                var lastDot = obj.Path.LastIndexOf(".");
                var extension = obj.Path.Substring(++lastDot);
                imageSource = ChoseBestImageSourceForThisExtention(extension);
            }

            var bitmapImage = new BitmapImage(imageSource);
            return bitmapImage;
        }

        private static Uri ChoseBestImageSourceForThisExtention(string extension)
        {
            Uri uri = null;
            switch (extension)
            {
                case "xaml":
                    uri = new Uri("/Images/XAML_file.png", UriKind.RelativeOrAbsolute);
                    break;
                case "cs":
                    uri = new Uri("/Images/cs.png", UriKind.RelativeOrAbsolute);
                    break;
            }
            return uri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}