using System;
using Gi7.Service;

namespace Gi7.Model.Feed
{
    public class FollowFeed : Gi7.Model.Feed.Base.Feed
    {
        public User Target { get; set; }

        public override string Template
        {
            get
            {
                return String.Format("started following {0}", Target.Login);
            }
        }

        public override string Image
        {
            get
            {
                return "/Gi7;component/Images/follow.png";
            }
        }
    }
}
