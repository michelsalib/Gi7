using System;
using System.Linq;

namespace Gi7.Client.Model
{
    public class Repository : BoolModel
    {
        public Repository()
        {
            Name = "";
            Owner = new User
            {
                Login = ""
            };
        }

        public string Url { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value.Contains("/"))
                {
                    Owner.Login = new string(value.TakeWhile(c => c != '/').ToArray());
                    name = new string(value.SkipWhile(c => c != '/').Skip(1).ToArray());
                }
                else
                {
                    name = value;
                }
            }
        }

        public User Owner { get; set; }
        public User Organization { get; set; }
        public string Description { get; set; }
        public bool HasIssues { get; set; }
        public string CloneUrl { get; set; }
        public Repository Parent { get; set; }
        public Repository Source { get; set; }
        public int Watchers { get; set; }
        public string GitUrl { get; set; }
        public bool HasWiki { get; set; }
        public bool Fork { get; set; }
        public bool IsFork { get { return !Fork; } }
        public bool NotFork { get { return Fork; } }
        public string Language { get; set; }
        public string Homepage { get; set; }
        public string SvnUrl { get; set; }
        public string MirrorUrl { get; set; }
        public bool HasDownloads { get; set; }
        public int Forks { get; set; }
        public int OpenIssues { get; set; }
        public string SshUrl { get; set; }
        public string HtmlUrl { get; set; }
        public string CurrentUser { get; set; }
        public string Fullname
        {
            get { return string.Format(CurrentUser != null && CurrentUser.Equals(Owner.Login, StringComparison.InvariantCultureIgnoreCase) ? "{1}" : "{0}/{1}", Owner.Login, Name); }
        }

        public override string ToString()
        {
            return Fullname;
        }
    }
}