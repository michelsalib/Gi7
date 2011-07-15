using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Github7.Model;
using Github7.Model.Feed;
using Github7.Utils;
using RestSharp;
using Microsoft.Practices.ServiceLocation;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;

namespace Github7.Service
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
            Username = "default";
            _client = new CachedClient("https://api.github.com", Username);

            String username = null;
            String password = null;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("username", out username) &&
                IsolatedStorageSettings.ApplicationSettings.TryGetValue("password", out password))
            {
                AuthenticateUser(username, password);
                IsAuthenticated = true;
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

        /// <summary>
        /// Gets the current user private news feed
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Feed> GetNewsFeed()
        {
            var feedClient = new CachedClient("https://github.com/", Username, _password);
            feedClient.AddHandler("application/json", new FeedDeserializer());

            var result = feedClient.GetList<Feed>(String.Format("{0}.private.json", Username));

            return result;
        }

        /// <summary>
        /// Gets the public news feed of the current user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ObservableCollection<Feed> GetNewsFeed(String username)
        {
            var feedClient = new CachedClient("https://github.com/", "");
            feedClient.AddHandler("application/json", new FeedDeserializer());

            var result = feedClient.GetList<Feed>(String.Format("{0}.json", username));

            return result;
        }

        public ObservableCollection<Repository> GetWatchedRepos(string username)
        {
            return _client.GetList<Repository>(String.Format("/users/{0}/watched", username));
        }

        public ObservableCollection<User> GetFollowers(string username)
        {
            return _client.GetList<User>(String.Format("/users/{0}/followers", username));
        }

        public ObservableCollection<User> GetFollowing(string username)
        {
            return _client.GetList<User>(String.Format("/users/{0}/following", username));
        }

        public Repository GetRepository(string username, string reponame, Action<Repository> callback)
        {
            return _client.Get<Repository>(String.Format("/repos/{0}/{1}", username, reponame), callback);
        }

        public User GetUser(string username, Action<User> callback)
        {
            return _client.Get<User>(String.Format("/users/{0}", username), callback);
        }
    }
}
