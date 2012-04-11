using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.PullRequest
{
    public class List : PaginatedRequest<Model.PullRequest>
    {
        public List(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/pulls", username, repo);
        }
    }
}