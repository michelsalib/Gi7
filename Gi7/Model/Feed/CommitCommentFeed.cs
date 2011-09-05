using System;
using Gi7.Service;
using Gi7.Model.Feed.Base;

namespace Gi7.Model.Feed
{
    public class CommitCommentFeed : RepositoryFeed
    {
        public String Commit { get; set; }

        public int CommentId { get; set; }

        public override String Template
        {
            get
            {
                return "commented commit on";
            }
        }

        public override String Image
        {
            get
            {
                return "/Gi7;component/Images/comment.png";
            }
        }

        public override String Destination
        {
            get
            {
                return String.Format(ViewModelLocator.CommitUrl, Repository.Owner.Login, Repository.Name, Commit);
            }
        }
    }
}
