using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Gi7.Utils.Converters
{
    public class EmptyConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = value as String;

            if (b != null && b.Trim().Length > 0)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}