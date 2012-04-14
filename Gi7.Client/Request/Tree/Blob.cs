using Gi7.Client.Request.Base;
using Gi7.Client.Utils;
using System;

namespace Gi7.Client.Request.Tree
{
    public class Blob : SingleRequest<Model.Blob>
    {
        public Blob(string username, string repo, string sha)
        {
            Uri = String.Format("/repos/{0}/{1}/git/blobs/{2}", username, repo, sha);
        }
    }
}
