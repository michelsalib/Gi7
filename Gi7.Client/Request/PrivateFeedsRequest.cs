using System;
using Gi7.Client.Model.Feed.Base;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;

namespace Gi7.Client.Request
{
    public class PrivateFeedsRequest : PaginatedRequest<Feed, Feed>
    {
        public PrivateFeedsRequest(String username)
        {
            Uri = String.Format("{0}.private.json", username);
            OverrideSettings = new OverrideSettings
            {
                BaseUri = "https://github.com/",
                Deserializer = new FeedDeserializer(),
                ContentType = "application/json",
            };
        }
    }
}