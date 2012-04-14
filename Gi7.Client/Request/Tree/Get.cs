using Gi7.Client.Request.Base;
using Gi7.Client.Utils;
using System;
using RestSharp;
using System.Linq;

namespace Gi7.Client.Request.Tree
{
    public class Get : SingleRequest<Model.GitTree>
    {
        public Get(string username, string repo, string sha)
        {
            Uri = String.Format("/repos/{0}/{1}/git/trees/{2}", username, repo, sha);
        }

        protected override Model.GitTree getData(RestResponse<Model.GitTree> response)
        {
            var data = response.Data;

            data.Tree = data.Tree.OrderByDescending(o => o.Type).ThenBy(o => o.Path).ToList();

            return data;
        }
    }
}
