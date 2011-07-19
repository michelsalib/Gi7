using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gi7.Model.Feed
{
    public class CommitCommentFeed : Feed
    {
        public String Commit { get; set; }

        public int CommentId { get; set; }

        public override String Template
        {
            get
            {
                return "commented on";
            }
        }

        public override String Image
        {
            get
            {
                return "/Gi7;component/Images/comment.png";
            }
        }
    }
}
