using System;

namespace Gi7.Model.Feed
{
    public class PullRequestFeed : Feed
    {
        public int Number { get; set; }

        public String Action { get; set; }

        public PullRequest PullRequest { get; set; }

        public override String Template
        {
            get
            {
                return String.Format("{0} pull request on", Action);
            }
        }

        public override String Image
        {
            get
            {
                return "/Gi7;component/Images/issues_opened.png";
            }
        }
    }
}
