using System;
using System.Windows.Data;
using Gi7.Client.Model.Event;

namespace Gi7.Utils.Converters
{
    public class EventConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var e = value as Event;

            if (e != null)
            {
                if ((String)parameter == "description")
                {
                    return new EventManager().GetDescription(e);
                }
                else if ((String)parameter == "image")
                {
                    return new EventManager().GetImage(e);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
