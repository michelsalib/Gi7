using Gi7.Client.Model;
using Gi7.Client.Request.Base;
using System;
using RestSharp;
using Newtonsoft.Json;

namespace Gi7.Client.Request.PullRequest
{
    public class Comment : SingleRequest<Model.Comment>
    {
        private AddData addData;

        public Comment(string username, string repo, string number, string body)
        {
            addData = new AddData() { Body = body.Replace('\r', '\n') };
            Uri = String.Format("/repos/{0}/{1}/pulls/{2}/comments", username, repo, number);
        }

        protected override void preRequest(RestSharp.RestClient client, RestSharp.RestRequest request)
        {
            request.Method = RestSharp.Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.AddBody(addData);
        }

        public class AddData {
            [JsonProperty("body")]
            public String Body { get; set; }
        }
    }
}
