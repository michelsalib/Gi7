using System;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
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
        private readonly IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;

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
            string username, password;
            if (isolatedStorageSettings.TryGetValue("username", out username) &&
                isolatedStorageSettings.TryGetValue("password", out password))
                AuthenticateUser(username, password);
            else
                Username = null;
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
                isolatedStorageSettings["username"] = username;
                isolatedStorageSettings["password"] = password;

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

            isolatedStorageSettings.Remove("username");
            isolatedStorageSettings.Remove("password");

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

        public Task<TResult> Load<TResult>(IRequest<TResult> request)
        {
            var taskCompletionSource = new TaskCompletionSource<TResult>();
            // prepare client
            var client = _createClient();

            bindRequest(request);

            // execute
            request.Execute(client, taskCompletionSource.SetResult);

            return taskCompletionSource.Task;
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