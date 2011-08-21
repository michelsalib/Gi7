using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gi7.Utils;
using RestSharp;

namespace Gi7.Service
{
    /// <summary>
    /// Specialization of the RestClient that uses the cache provider to answer more rapidly to requests
    /// </summary>
    public class CachedClient : RestClient
    {
        public CacheProvider CacheProvider { get; private set; }

        public CachedClient(String baseUri, String username, String password = "")
            : base (baseUri)
        {
            CacheProvider = new CacheProvider(username);

            if (!String.IsNullOrWhiteSpace(password))
                Authenticator = new HttpBasicAuthenticator(username, password);
        }

        public ObservableCollection<T> GetList<T>(String uri, Action<ObservableCollection<T>> callback = null, bool useCache = true)
            where T : new()
        {
            GlobalLoading.Instance.IsLoading = true;

            var result = new ObservableCollection<T>();

            if (useCache)
            {
                var cache = CacheProvider.Get<List<T>>(uri);
                if (cache != null)
                {
                    foreach (var item in cache)
                    {
                        result.Add(item);
                    }
                }
            }

            ExecuteAsync<List<T>>(new RestRequest(uri), r =>
            {
                result.Clear();
                foreach (var item in r.Data)
                {
                    result.Add(item);
                }

                if (callback != null)
                {
                    callback(result);
                }

                CacheProvider.Save(uri, r.Data.AsEnumerable());

                GlobalLoading.Instance.IsLoading = false;
            });

            return result;
        }

        public T Get<T>(string uri, Action<T> callback, bool useCache = true)
            where T : new()
        {
            GlobalLoading.Instance.IsLoading = true;
            T result;

            if (useCache)
            {
                var cache = CacheProvider.Get<T>(uri);
                result = cache != null ? cache : new T();
            }
            else
            {
                result = new T();
            }


            ExecuteAsync<T>(new RestRequest(uri), r =>
            {
                callback(r.Data);
                CacheProvider.Save(uri, r.Data);

                GlobalLoading.Instance.IsLoading = false;
            });

            return result;
        }

        public void ClearCache()
        {
            CacheProvider.Clear();
        }
    }
}
