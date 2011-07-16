using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;

namespace Github7.Service
{
    /// <summary>
    /// Specialization of the RestClient that uses the cache provider to answer more rapidly to requests
    /// </summary>
    public class CachedClient : RestClient
    {
        public bool IsLoading { get; private set; }

        public CacheProvider CacheProvider { get; private set; }

        public event EventHandler<LoadingEventArgs> Loading;

        private int _loadingCalls;

        public CachedClient(String baseUri, String username, String password = "")
            : base (baseUri)
        {
            _loadingCalls = 0;

            CacheProvider = new CacheProvider(username);

            if (!String.IsNullOrWhiteSpace(password))
                Authenticator = new HttpBasicAuthenticator(username, password);
        }

        public ObservableCollection<T> GetList<T>(String uri)
            where T : new()
        {
            _addCall();

            var result = new ObservableCollection<T>();

            var cache = CacheProvider.Get<List<T>>(uri);
            if (cache != null)
            {
                foreach (var item in cache)
                {
                    result.Add(item);
                }
            }

            ExecuteAsync<List<T>>(new RestRequest(uri), r =>
            {
                result.Clear();
                foreach (var item in r.Data)
                {
                    result.Add(item);
                }
                CacheProvider.Save(uri, r.Data.AsEnumerable());

                _endCall();
            });

            return result;
        }

        public T Get<T>(string uri, Action<T> callback)
            where T : new()
        {
            _addCall();
            var cache = CacheProvider.Get<T>(uri);

            var result = cache != null ? cache : new T();

            ExecuteAsync<T>(new RestRequest(uri), r =>
            {
                callback(r.Data);
                CacheProvider.Save(uri, r.Data);

                _endCall();
            });

            return result;
        }

        public void ClearCache()
        {
            CacheProvider.Clear();
        }

        private void _addCall()
        {
            if (_loadingCalls == 0)
            {
                IsLoading = true;
                if (Loading != null)
                    Loading(this, new LoadingEventArgs(IsLoading));
            }
            _loadingCalls++;
        }

        private void _endCall()
        {
            _loadingCalls--;
            if (_loadingCalls == 0)
            {
                IsLoading = false;
                if (Loading != null)
                    Loading(this, new LoadingEventArgs(IsLoading));
            }
        }
    }
}
