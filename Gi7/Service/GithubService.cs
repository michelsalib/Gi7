using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Net;
using Gi7.Model;
using Gi7.Service.Request.Base;
using RestSharp;

namespace Gi7.Service
{
    public class GithubService
    {
        public String Username { get; private set; }

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    if (IsAuthenticatedChanged != null)
                    {
                        IsAuthenticatedChanged(this, new AuthenticatedEventArgs(value));
                    }
                }
            }
        }

        public event EventHandler<AuthenticatedEventArgs> IsAuthenticatedChanged;

        private CachedClient _client;
        private String _password;

        /// <summary>
        /// The ctor will auto-authenticate if a username/password is in isolated storage
        /// </summary>
        public GithubService()
        {
            String username;
            String password;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("username", out username) &&
                IsolatedStorageSettings.ApplicationSettings.TryGetValue("password", out password))
            {
                AuthenticateUser(username, password);
                IsAuthenticated = true;
            }
            else
            {
                Username = "default";
                _client = new CachedClient("https://api.github.com", Username);
            }
        }

        /// <summary>
        /// Tries to authenticate and save the username/password in isolated storage
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void AuthenticateUser(String username, String password)
        {
            Username = username;
            _password = password;

            _client = new CachedClient("https://api.github.com", username, password);

            _client.ExecuteAsync<User>(new RestRequest("/user"), r =>
            {
                if (r.StatusCode != HttpStatusCode.NotFound &&
                    r.StatusCode != HttpStatusCode.Unauthorized)
                {
                    // set storage
                    IsolatedStorageSettings.ApplicationSettings["username"] = Username;
                    IsolatedStorageSettings.ApplicationSettings["password"] = _password;

                    IsAuthenticated = true;
                }
                else
                {
                    Logout();
                }
            });
        }

        /// <summary>
        /// Logout and clear the cache
        /// </summary>
        public void Logout()
        {
            Username = "";
            _password = "";

            IsolatedStorageSettings.ApplicationSettings.Remove("username");
            IsolatedStorageSettings.ApplicationSettings.Remove("password");

            _client.ClearCache();
            _client = new CachedClient("https://api.github.com", "");

            IsAuthenticated = false;
        }

        public ObservableCollection<T> Load<T>(IGithubPaginatedRequest<T> request, Action<ObservableCollection<T>> callback = null)
            where T : new()
        {
            if (request.OverrideSettings != null)
            {
                var overridenClient = new CachedClient(request.OverrideSettings.BaseUri, Username, _password);
                overridenClient.AddHandler("application/json", request.OverrideSettings.Deserializer);
                return overridenClient.GetList<T>(request.Uri, callback);
            }
            else
            {
                return _client.GetList<T>(request.Uri, callback, request.Page == 1); // use cache only for first page
            }
        }

        public T Load<T>(IGithubSingleRequest<T> request, Action<T> callback = null)
            where T : new()
        {
            if (request.OverrideSettings != null)
            {
                var overridenClient = new CachedClient(request.OverrideSettings.BaseUri, Username, _password);
                overridenClient.AddHandler("application/json", request.OverrideSettings.Deserializer);
                return overridenClient.Get<T>(request.Uri, callback);
            }
            else
            {
                return _client.Get<T>(request.Uri, callback);
            }
        }
    }
}
