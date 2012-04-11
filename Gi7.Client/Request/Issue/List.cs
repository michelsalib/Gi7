using System;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Issue
{
    public class List : PaginatedRequest<Model.Issue>
    {
        public List(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/issues", username, repo);
        }
    }
}