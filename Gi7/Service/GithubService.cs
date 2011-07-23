using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Gi7.Model;
using Gi7.Model.Feed;
using Gi7.Utils;
using RestSharp;
using Microsoft.Practices.ServiceLocation;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;

namespace Gi7.Service
{
    public class GithubService
    {
        public String Username { get; private set; }

        public bool IsLoading
        {
            get
            {
                if (_client != null)
                    return _client.IsLoading;
                return false;
            }
        } 

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
        public event EventHandler<LoadingEventArgs> Loading;

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
                _client.Loading += (s, e) => { if (Loading != null) Loading(this, e); };
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
            _client.Loading += (s, e) => { if (Loading != null) Loading(this, e); };

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
            _client.Loading += (s, e) => { if (Loading != null) Loading(this, e); };

            IsAuthenticated = false;
        }

        /// <summary>
        /// Gets the current user private news feed
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Feed> GetNewsFeed(int page = 1)
        {
            var feedClient = new CachedClient("https://github.com/", Username, _password);
            feedClient.AddHandler("application/json", new FeedDeserializer());

            var result = feedClient.GetList<Feed>(String.Format("{0}.private.json?page={1}", Username, page));

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

        public ObservableCollection<Push> GetCommits(string username, string repo)
        {
            return _client.GetList<Push>(String.Format("/repos/{0}/{1}/commits", username, repo));
        }

        public Push GetCommit(string username, string repo, string sha, Action<Push> callback)
        {
            return _client.Get<Push>(String.Format("/repos/{0}/{1}/commits/{2}", username, repo, sha), callback);
        }

        public ObservableCollection<Comment> GetCommitComments(string username, string repo, string sha)
        {
            return _client.GetList<Comment>(String.Format("/repos/{0}/{1}/commits/{2}/comments", username, repo, sha));
        }

        public ObservableCollection<PullRequest> GetPullRequests(string username, string repo)
        {
            return _client.GetList<PullRequest>(String.Format("/repos/{0}/{1}/pulls", username, repo));
        }

        public ObservableCollection<Issue> GetIssues(string username, string repo)
        {
            return _client.GetList<Issue>(String.Format("/repos/{0}/{1}/issues", username, repo));
        }
    }
}
