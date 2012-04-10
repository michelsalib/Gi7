using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class PullRequestsRequest : PaginatedRequest<PullRequest>
    {
        public PullRequestsRequest(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/pulls", username, repo);
        }
    }
}