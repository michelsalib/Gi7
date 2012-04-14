using System;

namespace Gi7.Client.Model
{
    public class Commit : BoolModel
    {
        public String Url { get; set; }

        public Committer Commiter { get; set; }

        public String Message { get; set; }

        public Committer Author { get; set; }

        public String Sha { get; set; }

        public GitTree Tree { get; set; }
    }
}