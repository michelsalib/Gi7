using System;
using System.Globalization;
using System.Windows.Data;

namespace Gi7.Utils.Converters
{
    public class GravatarSizeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = value as string;

            if (url != null)
                return String.Format("{0}?s={1}", url, parameter);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}