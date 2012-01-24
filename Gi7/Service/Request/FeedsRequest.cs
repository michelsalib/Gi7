using System;
using Gi7.Model.Feed.Base;
using Gi7.Service.Request.Base;
using Gi7.Utils;

namespace Gi7.Service.Request
{
    public class FeedsRequest : PaginatedRequest<Feed, Feed>
    {
        public FeedsRequest(String username)
        {
            Uri = String.Format("{0}.json", username);
            OverrideSettings = new OverrideSettings
            {
                BaseUri = "https://github.com/",
                Deserializer = new FeedDeserializer(),
                ContentType = "application/json",
            };
        }
    }
}