using System;
using Gi7.Service;

namespace Gi7.Model.Feed.Base
{
    public class RepositoryFeed : Feed
    {
        public Repository Repository { get; set; }

        public override String Destination
        {
            get { return String.Format(ViewModelLocator.RepositoryUrl, Repository.Owner.Login, Repository.Name); }
        }
    }
}