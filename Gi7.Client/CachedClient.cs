using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Gi7.Client
{
    /// <summary>
    /// Specialization of the RestClient that uses the cache provider to answer more rapidly to requests
    /// </summary>
    public class CachedClient : RestClient
    {
        public CachedClient(String baseUri, String email, String password)
            : base(baseUri)
        {
            CacheProvider = new CacheProvider(email);

            if (!String.IsNullOrWhiteSpace(password))
                Authenticator = new HttpBasicAuthenticator(email, password);
        }

        public CacheProvider CacheProvider { get; private set; }

        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;
        public event EventHandler<LoadingEventArgs> Loading;

        public List<T> GetList<T>(string uri, Action<List<T>> callback = null, bool useCache = true)
            where T : new()
        {
            return Get(uri, callback, useCache);
        }

        public T Get<T>(string uri, Action<T> callback = null, bool useCache = true)
            where T : new()
        {
            T result;

            if (useCache)
            {
                var cache = CacheProvider.Get<T>(uri);
                result = cache != null ? cache : new T();
            } else
                result = new T();

            var restRequest = new RestRequest(uri);
            ExecuteAsync<T>(restRequest, r =>
            {
                if (callback != null)
                    callback(r.Data);
                CacheProvider.Save(uri, r.Data);
            });

            return result;
        }

        public void ClearCache()
        {
            CacheProvider.Clear();
        }

        public override void ExecuteAsync<T>(RestRequest request, Action<RestResponse<T>> callback)
        {
            if (Loading != null)
                Loading(this, new LoadingEventArgs(true));
            base.ExecuteAsync<T>(request, r =>
            {
                if (Loading != null)
                    Loading(this, new LoadingEventArgs(false));
                if (r.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (Unauthorized != null)
                        Unauthorized(this, new EventArgs());
                } else if (r.ResponseStatus == ResponseStatus.Error)
                {
                    if (ConnectionError != null)
                        ConnectionError(this, new EventArgs());
                } else
                    callback(r);
            });
        }
    }
}