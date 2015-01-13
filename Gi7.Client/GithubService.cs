using System;
using System.IO.IsolatedStorage;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Client.Request.Base;
using Octokit;
using RestSharp;

namespace Gi7.Client
{
    public class GithubService
    {
        private bool _isAuthenticated;
        private String _password;
        public String Username { get; private set; }

        private Connection connection;

        public ApiConnection GitConnection
        {
            get { return new ApiConnection(connection);}
        }

        public event EventHandler<AuthenticatedEventArgs> IsAuthenticatedChanged;
        public event EventHandler<LoadingEventArgs> Loading;
        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    if (IsAuthenticatedChanged != null)
                        IsAuthenticatedChanged(this, new AuthenticatedEventArgs(value));
                }
            }
        }

        /// <summary>
        /// Init will auto-authenticate if a username/password is in isolated storage
        /// </summary>
        public void Init()
        {
            String username;
            String password;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("username", out username) &&
                IsolatedStorageSettings.ApplicationSettings.TryGetValue("password", out password))
            {
                AuthenticateUser(username, password);
            }
            else
            {
                Username = null;
            }
        }


        /// <summary>
        /// Tries to authenticate and save the email/password in isolated storage
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void AuthenticateUser(String username, String password)
        {
            Username = username;
            _password = password;

            var request = new UserRequest();

            Load(request, r =>
            {
                // set storage
                IsolatedStorageSettings.ApplicationSettings["username"] = username;
                IsolatedStorageSettings.ApplicationSettings["password"] = password;

                connection = new Connection(new ProductHeaderValue("Gi7"))
                {
                    Credentials = new Credentials(username, password)
                };

                IsAuthenticated = true;
            });
        }

        /// <summary>
        /// Logout and clear the cache
        /// </summary>
        public void Logout()
        {
            Username = null;
            _password = null;

            IsolatedStorageSettings.ApplicationSettings.Remove("username");
            IsolatedStorageSettings.ApplicationSettings.Remove("password");

            IsAuthenticated = false;
        }

        public TResult Load<TResult>(IRequest<TResult> request, Action<TResult> callback = null)
        {
            // prepare client
            var client = _createClient();

            bindRequest(request);

            // execute
            request.Execute(client, callback);

            return request.Result;
        }

        private RestClient _createClient()
        {
            var client = new RestClient("https://api.github.com");

            if (Username != null && _password != null)
            {
                client.Authenticator = new HttpBasicAuthenticator(Username, _password);
            }

            return client;
        }

        private void bindRequest<TResult>(IRequest<TResult> request)
        {
            request.ConnectionError += (s, e) =>
            {
                if (ConnectionError != null)
                    ConnectionError(this, new EventArgs());
            };
            request.Unauthorized += (s, e) =>
            {
                Logout();
                if (Unauthorized != null)
                    Unauthorized(this, new EventArgs());
            };
            request.Loading += (s, e) =>
            {
                if (Loading != null)
                    Loading(this, e);
            };
        }
    }
}