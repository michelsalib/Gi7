using System;
using Gi7.Client.Model.Event;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;

namespace Gi7.Client.Request
{
    public class ReceivedEventsRequest : PaginatedRequest<Event>
    {
        public ReceivedEventsRequest(String username)
        {
            Uri = String.Format("/users/{0}/received_events", username);
        }

        protected override void preRequest(RestSharp.RestClient client, RestSharp.RestRequest request)
        {
            client.AddHandler("application/json", new EventDeserializer());
        }
    }
}
