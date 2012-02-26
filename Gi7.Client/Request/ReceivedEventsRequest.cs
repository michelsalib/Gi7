using System;
using Gi7.Client.Model.Event;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;

namespace Gi7.Client.Request
{
    public class ReceivedEventsRequest : PaginatedRequest<Event, Event>
    {
        public ReceivedEventsRequest(String username)
        {
            Uri = String.Format("/users/{0}/received_events", username);
            OverrideSettings = new OverrideSettings
            {
                BaseUri = "https://api.github.com",
                Deserializer = new EventDeserializer(),
                ContentType = "application/json",
            };
        }
    }
}
