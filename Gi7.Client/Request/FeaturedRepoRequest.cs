using Gi7.Client.Model.Extra;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;

namespace Gi7.Client.Request
{
    public class FeaturedRepoRequest : PaginatedRequest<FeaturedRepo, FeaturedRepo>
    {
        public FeaturedRepoRequest()
        {
            Uri = "/thechangelog";
            OverrideSettings = new OverrideSettings
            {
                BaseUri = "http://feeds.feedburner.com/",
                Deserializer = new FeaturedRepoDeserializer(),
                ContentType = "text/xml",
            };
        }
    }
}