using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class IssueCommentsRequest : PaginatedRequest<Comment>
    {
        public IssueCommentsRequest(string username, string repo, string number)
        {
            Uri = String.Format("/repos/{0}/{1}/issues/{2}/comments", username, repo, number);
        }
    }
}