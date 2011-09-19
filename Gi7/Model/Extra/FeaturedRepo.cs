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

namespace Gi7.Model.Extra
{
    public class FeaturedRepo
    {
        public String Title { get; set; }

        public String User { get; set; }

        public String Repo { get; set; }

        public String Fullname
        {
            get
            {
                return String.Format("{0}/{1}", User, Repo);
            }
        }
    }
}
