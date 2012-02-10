using System;
using Gi7.Client.Model.Feed.Base;

namespace Gi7.Client.Model.Feed
{
    public class WatchFeed : RepositoryFeed
    {
        public String Action { get; set; }

        public override String Template
        {
            get { return String.Format("{0} watching on", Action); }
        }

        public override String Image
        {
            get { return null; }
        }
    }
}