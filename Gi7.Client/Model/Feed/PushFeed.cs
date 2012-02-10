using System;
using Gi7.Client.Model.Feed.Base;

namespace Gi7.Client.Model.Feed
{
    public class PushFeed : RepositoryFeed
    {
        public String Head { get; set; }

        public int Size { get; set; }

        public String Ref { get; set; }

        public override String Template
        {
            get { return "pushed on"; }
        }

        public override String Image
        {
            get { return "/Gi7;component/Images/push.png"; }
        }

        public override String Destination
        {
            get { return String.Format(DestinationFormat, Repository.Owner.Login, Repository.Name, Ref); }
        }
    }
}