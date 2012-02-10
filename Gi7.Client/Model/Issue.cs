using System;

namespace Gi7.Client.Model
{
    public class Issue : BoolModel
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

        public int Comments { get; set; }
    }
}