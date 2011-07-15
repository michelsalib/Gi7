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
    public class PullRequest
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public String Title { get; set; }

        public int Additions { get; set; }

        public int IssueId { get; set; }

        public int Commits { get; set; }

        public int Deletions { get; set; }
    }
}
