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
using Gi7.Service;

namespace Gi7.Model.Feed
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
                return "/Gi7;component/Images/push.png";
            }
        }

        public override String Destination
        {
            get
            {
                return String.Format(ViewModelLocator.CommitUrl, Repository.Owner.Login, Repository.Name, Ref);
            }
        }
    }
}
