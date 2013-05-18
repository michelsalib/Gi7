using System;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;

namespace Gi7.Client.Request.User
{
    public class Get : SingleRequest<Model.User>
    {
        public Get(string username)
        {
            Uri = String.Format("/users/{0}", username);
        }

        protected override void preRequest(RestSharp.RestClient client, RestSharp.RestRequest request)
        {
            client.AddHandler("application/json", new UserDeserializer());
        }
    }
}