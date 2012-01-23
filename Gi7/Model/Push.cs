using System;
using System.Collections.Generic;

namespace Gi7.Model
{
    public class Push
    {
        public String Url { get; set; }

        public User Committer { get; set; }

        public User Author { get; set; }

        public Commit Commit { get; set; }

        public String Sha { get; set; }

        public PushStats Stats { get; set; }

        public List<File> Files { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Push && ((Push)obj).Sha == Sha;
        }
    }
}