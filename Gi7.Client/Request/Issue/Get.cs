using System;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Issue
{
    public class Get : SingleRequest<Model.Issue>
    {
        public Get(string username, string repo, string number)
        {
            Uri = String.Format("/repos/{0}/{1}/issues/{2}", username, repo, number);
        }
    }
}