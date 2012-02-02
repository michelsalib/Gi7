using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class BranchesRequest : PaginatedRequest<Branch, Branch>
    {
        public BranchesRequest(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/branches", username, repo);
        }
    }
}
