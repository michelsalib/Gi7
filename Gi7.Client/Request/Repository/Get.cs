using System;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Repository
{
    public class Get : SingleRequest<Model.Repository>
    {
        public Get(string username, string reponame)
        {
            Uri = String.Format("/repos/{0}/{1}", username, reponame);
        }
    }
}
