using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class RepositoryRequest : SingleRequest<Repository>
    {
        public RepositoryRequest(string username, string reponame)
        {
            Uri = String.Format("/repos/{0}/{1}", username, reponame);
        }
    }
}