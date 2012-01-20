using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class PullRequestRequest : SingleRequest<PullRequest>
    {
        public PullRequestRequest(string username, string repo, string number)
        {
            Uri = String.Format("/repos/{0}/{1}/pulls/{2}", username, repo, number);
        }
    }
}