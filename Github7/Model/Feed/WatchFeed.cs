using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Github7.Model.Feed
{
    public class WatchFeed : Feed
    {
        public String Action { get; set; }

        public override String Template
        {
            get
            {
                return String.Format("{0} watching on", Action);
            }
        }

        public override String Image
        {
            get
            {
                return null;
            }
        }
    }
}
