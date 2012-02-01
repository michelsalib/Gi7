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
            var b = (string)value;

            return b.Substring(0, Math.Min(b.Length, 65)).Replace("\n", ". ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}