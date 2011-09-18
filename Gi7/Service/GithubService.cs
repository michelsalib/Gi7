using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Net;
using Gi7.Model;
using Gi7.Service.Request.Base;
using RestSharp;
using System.Collections.Generic;
using System.Windows;

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
        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;

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
                _client = _createClient("https://api.github.com", Username, "");
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

            _client = _createClient("https://api.github.com", username, password);

            _client.ExecuteAsync<User>(new RestRequest("/user"), r =>
            {
                // set storage
                IsolatedStorageSettings.ApplicationSettings["username"] = Username;
                IsolatedStorageSettings.ApplicationSettings["password"] = _password;

                IsAuthenticated = true;
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
            _client = _createClient("https://api.github.com", "", "");

            IsAuthenticated = false;
        }

        public ObservableCollection<T> Load<T>(IGithubPaginatedRequest<T> request, Action<List<T>> callback = null)
            where T : new()
        {
            // prepare client
            CachedClient client;
            if (request.OverrideSettings != null)
            {
                client = _createClient(request.OverrideSettings.BaseUri, Username, _password);
                client.AddHandler("application/json", request.OverrideSettings.Deserializer);
            }
            else
            {
                client = _client;
            }

            request.Page++;
            // if page is 1, we need to set the collection and use cache
            if (request.Page == 1)
            {
                request.Result = new ObservableCollection<T>(client.GetList<T>(request.Uri, r =>
                {
                    request.Result.Clear();
                    if (r.Count < 30)
                    {
                        request.HasMoreItems = false;
                    }
                    foreach (var i in r)
                    {
                        request.Result.Add(i);
                    }
                    if (callback != null)
                    {
                        callback(r);
                    }
                }, true));
            }
            // else the collection already exists and there is no cache
            else
            {
                client.GetList<T>(request.Uri, r =>
                {
                    if (r.Count < 30)
                    {
                        request.HasMoreItems = false;
                    }
                    foreach (var i in r)
                    {
                        request.Result.Add(i);
                    }
                    if (callback != null)
                    {
                        callback(r);
                    }
                }, true);
            }

            return request.Result;
        }

        public T Load<T>(IGithubSingleRequest<T> request, Action<T> callback = null)
            where T : new()
        {
            // prepare the client
            CachedClient client;
            if (request.OverrideSettings != null)
            {
                client = _createClient(request.OverrideSettings.BaseUri, Username, _password);
                client.AddHandler("application/json", request.OverrideSettings.Deserializer);
            }
            else
            {
                client = _client;
            }

            request.Result = client.Get<T>(request.Uri, r =>
            {
                request.Result = r;
                if (callback != null)
                {
                    callback(r);
                }
            }, true);

            return request.Result;
        }

        private CachedClient _createClient(String baseUri, String usernamne, String password)
        {
            var client = new CachedClient(baseUri, usernamne, password);
            client.ConnectionError += (s, e) =>
            {
                MessageBox.Show("Server unreachable.");
                if (ConnectionError != null)
                    ConnectionError(this, new EventArgs());
            };
            client.Unauthorized += (s, e) =>
            {
                MessageBox.Show("Wrong credentials.");
                Logout();
                if(Unauthorized != null)
                    Unauthorized(this, new EventArgs());
            };

            return client;
        }
    }
}
