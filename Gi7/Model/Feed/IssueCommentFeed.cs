using System;
using Gi7.Model.Feed.Base;
using Gi7.Service;

namespace Gi7.Model.Feed
{
    public class IssueCommentFeed : RepositoryFeed
    {
        public int IssueId { get; set; }

        public int CommentId { get; set; }

        public override String Template
        {
            get
            {
                return "commented issue on";
            }
        }

        public override String Image
        {
            get
            {
                return "/Gi7;component/Images/issues_comment.png";
            }
        }

        public override String Destination
        {
            get
            {
                return String.Format(ViewModelLocator.IssueUrl, Repository.Owner.Login, Repository.Name, new System.Text.RegularExpressions.Regex("/issues/(\\d+)").Match(Url).Groups[1].Value);
            }
        }
    }
}
