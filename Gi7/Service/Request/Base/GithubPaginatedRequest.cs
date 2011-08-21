using System;

namespace Gi7.Service.Request.Base
{
    public abstract class GithubPaginatedRequest<T> : IGithubPaginatedRequest<T>
        where T : new ()
    {
        public int Page { get; set; }

        private string _uri;
        public string Uri
        {
            get
            {
                return String.Format("{0}?page={1}", _uri, Page);
            }
            protected set
            {
                _uri = value;
            }
        }

        public OverrideSettings OverrideSettings { get; protected set; }

        public GithubPaginatedRequest()
        {
            Page = 1;
        }
    }
}
