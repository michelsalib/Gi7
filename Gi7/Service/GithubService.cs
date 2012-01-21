using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Windows;
using Gi7.Model;
using Gi7.Service.Request.Base;
using Gi7.Utils;
using RestSharp;

namespace Gi7.Service
{
    public class GithubService
    {
        private CachedClient _client;
        private bool _isAuthenticated;
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

        public String Username { get; private set; }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
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

        public ObservableCollection<T> Load<T>(IPaginatedRequest<T> request, Action<List<T>> callback = null)
            where T : new()
        {
            // prepare client
            CachedClient client;
            if (request.OverrideSettings != null)
            {
                client = _createClient(request.OverrideSettings.BaseUri, Username, _password);
                client.AddHandler(request.OverrideSettings.ContentType, request.OverrideSettings.Deserializer);
            }
            else
            {
                client = _client;
            }

            request.Page++;
            // if page is 1, we need to set the collection and use cache
            if (request.Page == 1)
            {
                request.Result = new BetterObservableCollection<T>(client.GetList<T>(request.Uri, r =>
                {
                    request.Result.Clear();
                    if (r.Count < 30)
                        request.HasMoreItems = false;

                    request.Result.AddRange(r);

                    if (callback != null)
                        callback(r);
                }));
            } // else the collection already exists and there is no cache
            else
            {
                client.GetList<T>(request.Uri, r =>
                {
                    if (r.Count < 30)
                        request.HasMoreItems = false;

                    request.Result.AddRange(r);

                    if (callback != null)
                        callback(r);
                });
            }

            return request.Result;
        }

        public T Load<T>(ISingleRequest<T> request, Action<T> callback = null)
            where T : new()
        {
            // prepare the client
            CachedClient client;
            if (request.OverrideSettings != null)
            {
                client = _createClient(request.OverrideSettings.BaseUri, Username, _password);
                client.AddHandler(request.OverrideSettings.ContentType, request.OverrideSettings.Deserializer);
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
            });

            return request.Result;
        }

        private CachedClient _createClient(String baseUri, String usernamne, String password)
        {
            var client = new CachedClient(baseUri, usernamne, password);
            client.ConnectionError += (s, e) =>
            {
                MessageBox.Show("Server unreachable.", "Gi7", MessageBoxButton.OK);
                if (ConnectionError != null)
                    ConnectionError(this, new EventArgs());
            };
            client.Unauthorized += (s, e) =>
            {
                MessageBox.Show("Wrong credentials.", "Gi7", MessageBoxButton.OK);
                Logout();
                if (Unauthorized != null)
                    Unauthorized(this, new EventArgs());
            };

            return client;
        }
    }
}