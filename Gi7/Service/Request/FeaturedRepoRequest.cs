using Gi7.Model.Extra;
using Gi7.Service.Request.Base;
using Gi7.Utils;

namespace Gi7.Service.Request
{
    public class FeaturedRepoRequest : PaginatedRequest<FeaturedRepo>
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