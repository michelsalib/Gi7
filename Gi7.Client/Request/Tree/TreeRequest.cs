using System;
using System.Linq;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;
using RestSharp;

namespace Gi7.Client.Request
{
    public class TreeRequest : SingleRequest<GitTree>
    {
        public TreeRequest(string username, string repo, string sha)
        {
            Uri = String.Format("/repos/{0}/{1}/git/trees/{2}", username, repo, sha);
        }

        protected override GitTree GetData(IRestResponse<GitTree> response)
        {
            var data = response.Data;

            data.Tree = data.Tree.OrderByDescending(o => o.Type).ThenBy(o => o.Path).ToList();

            return data;
        }
    }
}