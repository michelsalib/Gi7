using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class IssuesRequest : PaginatedRequest<Issue, Issue>
    {
        public IssuesRequest(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/issues", username, repo);
        }
    }
}