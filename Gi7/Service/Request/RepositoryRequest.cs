using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class RepositoryRequest : GithubSingleRequest<Repository>
    {
        public RepositoryRequest(string username, string reponame)
        {
            Uri = String.Format("/repos/{0}/{1}", username, reponame);
        }
    }
}
