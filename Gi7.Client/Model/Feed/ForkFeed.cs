using System;
using Gi7.Client.Model.Feed.Base;

namespace Gi7.Client.Model.Feed
{
    public class ForkFeed : RepositoryFeed
    {
        public override String Template
        {
            get { return "forked"; }
        }

        public override String Image
        {
            get { return "/Gi7;component/Images/fork.png"; }
        }
    }
}