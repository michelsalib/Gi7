using System;
using Gi7.Client.Model.Event;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;
using RestSharp;

namespace Gi7.Client.Request
{
    public class UserReceivedEventsRequest : PaginatedRequest<Event>
    {
        public UserReceivedEventsRequest(String username)
        {
            Uri = String.Format("/users/{0}/received_events", username);
        }

        protected override void preRequest(RestClient client, RestRequest request)
        {
            client.AddHandler("application/json", new EventDeserializer());
        }
    }
}