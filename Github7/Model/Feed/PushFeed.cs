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
    public class PushFeed : Feed
    {
        public String Head { get; set; }

        public int Size { get; set; }

        public String Ref { get; set; }

        public override String Template
        {
            get
            {
                return "pushed on";
            }
        }

        public override String Image
        {
            get
            {
                return "/Github7;component/Images/push.png";
            }
        }
    }
}
