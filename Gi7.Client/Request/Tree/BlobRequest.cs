using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class BlobRequest : SingleRequest<Blob>
    {
        public BlobRequest(string username, string repo, string sha)
        {
            Uri = String.Format("/repos/{0}/{1}/git/blobs/{2}", username, repo, sha);
        }
    }
}