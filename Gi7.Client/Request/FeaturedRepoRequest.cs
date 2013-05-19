using Gi7.Client.Model.Extra;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;
using RestSharp;

namespace Gi7.Client.Request
{
    public class FeaturedRepoRequest : PaginatedRequest<FeaturedRepo>
    {
        public FeaturedRepoRequest()
        {
            Uri = "/thechangelog";
        }

        protected override void preRequest(RestClient client, RestRequest request)
        {
            client.BaseUrl = "http://feeds.feedburner.com/";
            client.AddHandler("text/xml", new FeaturedRepoDeserializer());
        }
    }
}