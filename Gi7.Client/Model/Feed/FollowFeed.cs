using System;

namespace Gi7.Client.Model.Feed
{
    public class FollowFeed : Base.Feed
    {
        public User Target { get; set; }

        public override string Template
        {
            get { return String.Format("started following {0}", Target.Login); }
        }

        public override string Image
        {
            get { return "/Gi7;component/Images/follow.png"; }
        }
    }
}