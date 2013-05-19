using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;
using RestSharp;

namespace Gi7.Client.Request
{
    public class UserRequest : SingleRequest<User>
    {
        public UserRequest(string username)
        {
            Uri = String.Format("/users/{0}", username);
        }

        protected override void preRequest(RestClient client, RestRequest request)
        {
            client.AddHandler("application/json", new UserDeserializer());
        }
    }
}