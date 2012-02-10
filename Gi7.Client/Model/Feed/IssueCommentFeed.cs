using System;
using System.Text.RegularExpressions;
using Gi7.Client.Model.Feed.Base;

namespace Gi7.Client.Model.Feed
{
    public class IssueCommentFeed : RepositoryFeed
    {
        public int IssueId { get; set; }

        public int CommentId { get; set; }

        public override String Template
        {
            get { return "commented issue on"; }
        }

        public override String Image
        {
            get { return "/Gi7;component/Images/issues_comment.png"; }
        }

        public override String Destination
        {
            get { return String.Format(DestinationFormat, Repository.Owner.Login, Repository.Name, new Regex("/issues/(\\d+)").Match(Url).Groups[1].Value); }
        }
    }
}