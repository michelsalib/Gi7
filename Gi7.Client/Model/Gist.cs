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
using System.Collections.Generic;

namespace Gi7.Client.Model
{
    public class Gist : BoolModel
    {
        public String Url { get; set; }

        public String Id { get; set; }

        public String Description { get; set; }

        public bool Public { get; set; }

        public User User { get; set; }

        //public List<File> Files { get; set; }

        public int Comments { get; set; }

        public String HtmlUrl { get; set; }

        public String GitPullUrl { get; set; }

        public String GitPushUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
