using System;
using Gi7.Client.Model.Feed.Base;

namespace Gi7.Client.Model.Feed
{
    public class CommitCommentFeed : RepositoryFeed
    {
        public String Commit { get; set; }

        public int CommentId { get; set; }

        public override String Template
        {
            get { return "commented commit on"; }
        }

        public override String Image
        {
            get { return "/Gi7;component/Images/comment.png"; }
        }

        public override String Destination
        {
            get { return String.Format(DestinationFormat, Repository.Owner.Login, Repository.Name, Commit); }
        }
    }
}