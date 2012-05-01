using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Phone.Shell;

namespace Gi7.Utils.Converters
{
    public class BoolToModeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = value as bool?;

            return (b.HasValue && b.Value) ? ApplicationBarMode.Minimized : ApplicationBarMode.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}