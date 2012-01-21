using System;
using System.Globalization;
using System.Windows.Data;

namespace Gi7.Utils.Converters
{
    public class TextTrimConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = value as string;

            if (b != null && b.Length > 65 ) 
                return b.Substring(0, 65).Insert(65,"...").Replace("\n", ". ");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}