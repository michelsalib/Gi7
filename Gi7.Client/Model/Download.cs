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

namespace Gi7.Client.Model
{
    public class Download : BoolModel
    {
        public String Url { get; set; }

        public String HtmlUrl { get; set; }

        public int Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public int Size { get; set; }

        public int DownloadCount { get; set; }

        public String ContentType { get; set; }
    }
}
