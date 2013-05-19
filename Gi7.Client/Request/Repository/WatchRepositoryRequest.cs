using System;
using System.Net;
using Gi7.Client.Request.Base;
using RestSharp;

namespace Gi7.Client.Request
{
    public class WatchRepositoryRequest : SingleRequest<bool?>
    {
        public enum Type
        {
            READ,
            WATCH,
            UNWATCH
        };

        private readonly Type _type;

        public WatchRepositoryRequest(string username, string reponame, Type type = Type.READ)
        {
            Uri = String.Format("/user/watched/{0}/{1}", username, reponame);
            _type = type;
        }

        public override void Execute(RestClient client, Action<bool?> callback = null)
        {
            var request = new RestRequest(Uri);

            switch (_type)
            {
            case Type.READ:
                request.Method = Method.GET;
                break;
            case Type.WATCH:
                request.Method = Method.PUT;
                break;
            case Type.UNWATCH:
                request.Method = Method.DELETE;
                break;
            }

            RaiseLoading(true);

            client.ExecuteAsync(request, r =>
            {
                RaiseLoading(false);

                if (r.StatusCode == HttpStatusCode.Unauthorized)
                    RaiseUnauthorized();
                else if (r.StatusCode == HttpStatusCode.NoContent)
                {
                    RaiseSuccess(true);

                    if (callback != null)
                        callback(true);
                } else if (r.StatusCode == HttpStatusCode.NotFound && _type == Type.READ)
                {
                    RaiseSuccess(false);
                    if (callback != null)
                        callback(false);
                } else
                    RaiseConnectionError();
            });
        }
    }
}