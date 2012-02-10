using System;
using Gi7.Client.Model.Feed.Base;

namespace Gi7.Client.Model.Feed
{
    public class PullRequestFeed : RepositoryFeed
    {
        public int Number { get; set; }

        public String Action { get; set; }

        public PullRequest PullRequest { get; set; }

        public override String Template
        {
            get { return String.Format("{0} pull request on", Action); }
        }

        public override String Image
        {
            get { return "/Gi7;component/Images/issues_opened.png"; }
        }
    }
}