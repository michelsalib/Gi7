using System;
using Gi7.Client.Request.Base;
using Newtonsoft.Json;
using RestSharp;

namespace Gi7.Client.Request.Issue
{
    public class CreateIssueRequest : SingleRequest<Model.Issue>
    {
        private readonly AddData addData;

        public CreateIssueRequest(string username, string repo, string title, string body)
        {
            addData = new AddData {Title = title, Body = body.Replace('\r', '\n')};
            Uri = String.Format("/repos/{0}/{1}/issues", username, repo);
        }

        protected override void preRequest(RestClient client, RestRequest request)
        {
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.AddBody(addData);
        }

        public class AddData
        {
            [JsonProperty("title")]
            public String Title { get; set; }

            [JsonProperty("body")]
            public String Body { get; set; }
        }
    }
}