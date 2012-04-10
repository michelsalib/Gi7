using System;
using GalaSoft.MvvmLight;
using RestSharp;
using System.Net;

namespace Gi7.Client.Request.Base
{
    public abstract class SingleRequest<TResult> : ViewModelBase, IRequest<TResult>
        where TResult : new()
    {
        public event EventHandler Success;
        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;
        public event EventHandler<LoadingEventArgs> Loading;
        public event EventHandler<NewResultEventArgs<TResult>> NewResult;

        private TResult _result;

        public string Uri { get; protected set; }

        public TResult Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        public virtual TResult Execute(RestClient client, Action<TResult> callback = null)
        {
            var request = new RestRequest(Uri);

            preRequest(client, request);

            if (Loading != null)
            {
                Loading(this, new LoadingEventArgs(true));
            }

            client.ExecuteAsync<TResult>(request, r =>
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
                else if (r.ResponseStatus == ResponseStatus.Error)
                {
                    if (ConnectionError != null)
                        ConnectionError(this, new EventArgs());
                }
                else
                {
                    if (Success != null)
                        Success(this, new EventArgs());

                    Result = r.Data;

                    if (NewResult != null)
                    {
                        NewResult(this, new NewResultEventArgs<TResult>()
                        {
                            NewResult = r.Data,
                        });
                    }

                    if (callback != null)
                    {
                        callback(r.Data);
                    }
                }
            });

            return Result;
        }

        protected virtual void preRequest(RestClient client, RestRequest request)
        {

        }
    }
}