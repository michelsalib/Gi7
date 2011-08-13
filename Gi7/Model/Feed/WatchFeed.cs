using System;

namespace Gi7.Model.Feed
{
    public class WatchFeed : Feed
    {
        public String Action { get; set; }

        public override String Template
        {
            get
            {
                return String.Format("{0} watching on", Action);
            }
        }

        public override String Image
        {
            get
            {
                return null;
            }
        }
    }
}
