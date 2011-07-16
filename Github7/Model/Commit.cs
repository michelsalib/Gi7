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
    public class Commit
    {
        public String Url { get; set; }

        public Committer Commiter { get; set; }

        public String Message { get; set; }

        public Committer Author { get; set; }
    }
}