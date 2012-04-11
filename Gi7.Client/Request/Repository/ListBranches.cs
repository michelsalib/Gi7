using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Repository
{
    public class ListBranches : PaginatedRequest<Branch>
    {
        public ListBranches(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/branches", username, repo);
        }
    }
}
