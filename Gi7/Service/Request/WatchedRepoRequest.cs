using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class WatchedRepoRequest : PaginatedRequest<Repository>
    {
        public WatchedRepoRequest(string username)
        {
            Uri = String.Format("/users/{0}/watched", username);
        }
    }
}