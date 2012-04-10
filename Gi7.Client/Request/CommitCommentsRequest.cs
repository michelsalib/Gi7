using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class CommitCommentsRequest : PaginatedRequest<Comment>
    {
        public CommitCommentsRequest(string username, string repo, string sha)
        {
            Uri = String.Format("/repos/{0}/{1}/commits/{2}/comments", username, repo, sha);
        }
    }
}