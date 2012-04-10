using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;
using GalaSoft.MvvmLight;
using RestSharp;
using System.Net;

namespace Gi7.Client.Request
{
    public class UserFollowingRequest : ViewModelBase, IRequest<bool?>
    {
        public event EventHandler Success;
        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;
        public event EventHandler<LoadingEventArgs> Loading;
        public event EventHandler<NewResultEventArgs<bool?>> NewResult;

        private Type _type;
        private bool? _result;

        public string Uri { get; protected set; }

        public bool? Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        public UserFollowingRequest(string username, Type type = Type.READ)
        {
            Uri = String.Format("/user/following/{0}", username);
            _type = type;
        }

        public virtual bool? Execute(RestClient client, Action<bool?> callback = null)
        {
            var request = new RestRequest(Uri);

            switch (_type)
            {
                case Type.READ:
                    request.Method = Method.GET;
                    break;
                case Type.FOLLOW:
                    request.Method = Method.PUT;
                    break;
                case Type.UNFOLLOW:
                    request.Method = Method.DELETE;
                    break;
            }

            if (Loading != null)
            {
                Loading(this, new LoadingEventArgs(true));
            }

            client.ExecuteAsync(request, r =>
            {
                if (Loading != null)
                {
                    Loading(this, new LoadingEventArgs(false));
                }

                if (r.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (Unauthorized != null)
                        Unauthorized(this, new EventArgs());
                }
                else if (r.StatusCode == HttpStatusCode.NoContent)
                {
                    if (Success != null)
                        Success(this, new EventArgs());

                    newResult(true);
                    callback(true);
                }
                else if (r.StatusCode == HttpStatusCode.NotFound && _type == Type.READ)
                {
                    if (Success != null)
                        Success(this, new EventArgs());

                    newResult(false);
                    callback(false);
                }
                else
                {
                    if (ConnectionError != null)
                        ConnectionError(this, new EventArgs());
                }
            });

            return null;
        }

        protected void newResult(bool? result)
        {
            if (NewResult != null)
            {
                NewResult(this, new NewResultEventArgs<bool?>()
                {
                    NewResult = result,
                });
            }
        }

        public enum Type { READ, FOLLOW, UNFOLLOW };
    }
}