using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;
using Newtonsoft.Json;
using RestSharp;

namespace Gi7.Client.Request
{
    public class CommentCommitRequest : SingleRequest<Comment>
    {
        private readonly AddData addData;

        public CommentCommitRequest(string username, string repo, string sha, string body)
        {
            addData = new AddData {Body = body.Replace('\r', '\n')};
            Uri = String.Format("/repos/{0}/{1}/commits/{2}/comments", username, repo, sha);
        }

        protected override void preRequest(RestClient client, RestRequest request)
        {
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.AddBody(addData);
        }

        public class AddData
        {
            [JsonProperty("body")]
            public String Body { get; set; }
        }
    }
}