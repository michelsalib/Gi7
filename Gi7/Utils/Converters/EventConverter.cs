using System;
using System.Windows.Data;
using Gi7.Client.Model.Event;
using System.Text.RegularExpressions;

namespace Gi7.Utils.Converters
{
    public class EventConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var e = value as Event;

            if (e != null)
            {
                if ((String)parameter == "title")
                {
                    return new EventManager().GetTitle(e);
                }
                if ((String)parameter == "description")
                {
                    var description = new EventManager().GetDescription(e);

                    if (description != null) {
                        description = new Regex("\\s+").Replace(description, " ");
                        if (description.Length > 128) {
                            description = description.Substring(0, 128).Trim() + "...";
                        }
                    }

                    return description;
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
