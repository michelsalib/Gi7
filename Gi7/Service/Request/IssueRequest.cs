using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class IssueRequest : SingleRequest<Issue, Issue>
    {
        public IssueRequest(string username, string repo, string number)
        {
            Uri = String.Format("/repos/{0}/{1}/issues/{2}", username, repo, number);
        }
    }
}