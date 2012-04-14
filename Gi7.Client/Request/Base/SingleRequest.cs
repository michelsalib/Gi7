using System;
using System.Net;
using GalaSoft.MvvmLight;
using RestSharp;

namespace Gi7.Client.Request.Base
{
    public abstract class SingleRequest<TResult> : ViewModelBase, IRequest<TResult>
        where TResult : new()
    {
        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;
        public event EventHandler<LoadingEventArgs> Loading;
        public event EventHandler<SuccessEventArgs<TResult>> Success;

        private TResult _result;

        public virtual string Uri { get; protected set; }

        public TResult Result
        {
            get { return _result; }
            protected set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        public virtual void Execute(RestClient client, Action<TResult> callback = null)
        {
            var request = new RestRequest(Uri);

            preRequest(client, request);

            RaiseLoading(true);

            client.ExecuteAsync<TResult>(request, r =>
            {
                RaiseLoading(false);

                if (r.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RaiseUnauthorized();
                }
                else if (r.ResponseStatus == ResponseStatus.Error)
                {
                    RaiseConnectionError();
                }
                else
                {
                    var data = getData(r);

                    Result = data;

                    RaiseSuccess(data);

                    if (callback != null)
                    {
                        callback(data);
                    }
                }
            });
        }

        protected virtual TResult getData(RestResponse<TResult> response)
        {
            return response.Data;
        }

        protected virtual void preRequest(RestClient client, RestRequest request)
        {

        }

        protected void RaiseLoading(bool isLoading)
        {
            if (Loading != null)
            {
                Loading(this, new LoadingEventArgs(isLoading));
            }
        }

        protected void RaiseUnauthorized()
        {
            if (Unauthorized != null)
            {
                Unauthorized(this, new EventArgs());
            }
        }

        protected void RaiseConnectionError()
        {
            if (ConnectionError != null)
            {
                ConnectionError(this, new EventArgs());
            }
        }

        protected void RaiseSuccess(TResult result)
        {
            if (Success != null)
            {
                Success(this, new SuccessEventArgs<TResult>()
                {
                    NewResult = result,
                });
            }
        }
    }
}