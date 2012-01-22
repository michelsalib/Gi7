using System;
using System.Text;

namespace Gi7.Model
{
    public class Commit
    {
        public String Url { get; set; }

        public Committer Commiter { get; set; }

        public String Message { get; set; }

        public Committer Author { get; set; }
    }
}