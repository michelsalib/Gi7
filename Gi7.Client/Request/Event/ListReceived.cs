using System;
using Gi7.Client.Model.Event;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;

namespace Gi7.Client.Request.Event
{
    public class ListReceived : PaginatedRequest<Model.Event.Event>
    {
        public ListReceived(String username)
        {
            Uri = String.Format("/users/{0}/received_events", username);
        }

        protected override void preRequest(RestSharp.RestClient client, RestSharp.RestRequest request)
        {
            client.AddHandler("application/json", new EventDeserializer());
        }
    }
}
