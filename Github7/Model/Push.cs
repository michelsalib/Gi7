using System;
using System.Collections.ObjectModel;

namespace Github7.Model
{
    public class Push
    {
        public String Url { get; set; }

        public User Committer { get; set; }

        public User Author { get; set; }

        public Commit Commit { get; set; }

        public String Sha { get; set; }
    }
}
