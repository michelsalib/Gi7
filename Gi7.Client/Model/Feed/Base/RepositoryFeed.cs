using System;

namespace Gi7.Client.Model.Feed.Base
{
    public class RepositoryFeed : Feed
    {
        public Repository Repository { get; set; }

        public override String Destination
        {
            get { return String.Format(DestinationFormat, Repository.Owner.Login, Repository.Name); }
        }
    }
}
