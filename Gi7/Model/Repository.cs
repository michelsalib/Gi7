using System;

namespace Gi7.Model
{
    public class Repository
    {
        public Repository()
        {
            Name = "";
            Owner = new User
            {
                Login = ""
            };
        }

        public String Url { get; set; }

        public String Name { get; set; }

        public User Owner { get; set; }

        public User Organization { get; set; }

        public String Description { get; set; }

        public bool HasIssues { get; set; }

        public String CloneUrl { get; set; }

        public Repository Parent { get; set; }

        public Repository Source { get; set; }

        public int Watchers { get; set; }

        public String GitUrl { get; set; }

        public bool HasWiki { get; set; }

        public String Homepage { get; set; }

        public String SvnUrl { get; set; }

        public String MirrorUrl { get; set; }

        public bool HasDownloads { get; set; }

        public int Forks { get; set; }

        public int OpenIssues { get; set; }

        public String SshUrl { get; set; }

        public String HtmlUrl { get; set; }

        public String Fullname
        {
            get { return String.Format("{0}/{1}", Owner.Login, Name); }
        }
    }
}