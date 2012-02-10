using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class BranchesRequest : PaginatedRequest<Branch, Branch>
    {
        public BranchesRequest(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/branches", username, repo);
        }
    }
}
