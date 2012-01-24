using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class PullRequestsRequest : PaginatedRequest<PullRequest, PullRequest>
    {
        public PullRequestsRequest(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/pulls", username, repo);
        }
    }
}