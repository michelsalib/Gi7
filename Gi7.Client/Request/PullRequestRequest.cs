using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class PullRequestRequest : SingleRequest<PullRequest, PullRequest>
    {
        public PullRequestRequest(string username, string repo, string number)
        {
            Uri = String.Format("/repos/{0}/{1}/pulls/{2}", username, repo, number);
        }
    }
}