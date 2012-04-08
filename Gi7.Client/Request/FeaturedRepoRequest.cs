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
        }

        protected override void preRequest(RestSharp.RestClient client, RestSharp.RestRequest request)
        {
            client.BaseUrl = "http://feeds.feedburner.com/";
            client.AddHandler("text/xml", new FeaturedRepoDeserializer());
        }
    }
}