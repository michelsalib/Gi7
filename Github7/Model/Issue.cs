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

namespace Github7.Model
{
    public class Issue
    {
        public PullRequest PullRequest { get; set; }

        public DateTime UpdatedAt { get; set; }

        public String Body { get; set; }

        public String Url { get; set; }

        public User User { get; set; }

        public String HtmlUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public String State { get; set; }

        public DateTime ClosedAt { get; set; }

        public String Title { get; set; }

        public int Number { get; set; }
    }
}
