using System;
using GalaSoft.MvvmLight;
using RestSharp;
using System.Net;

namespace Gi7.Client.Request.Base
{
    public abstract class SingleRequest<TSource, TDestination> : ViewModelBase, ISingleRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        public event EventHandler Success;
        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;
        public event EventHandler<LoadingEventArgs> Loading;
        public event EventHandler<NewResultEventArgs<TDestination>> NewResult;

        private TDestination _result;

        public string Uri { get; protected set; }

        public TDestination Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        public virtual TDestination Execute(RestClient client, Action<TDestination> callback = null)
        {
            var request = new RestRequest(Uri);

            preRequest(client, request);

            if (Loading != null)
            {
                Loading(this, new LoadingEventArgs(true));
            }

            client.ExecuteAsync<TSource>(request, r =>
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

                    var cast = SetResult(r.Data);

                    if (callback != null)
                    {
                        callback(cast);
                    }
                }
            });

            return Result;
        }

        protected virtual void preRequest(RestClient client, RestRequest request)
        {

        }

        public virtual TDestination SetResult(TSource result)
        {
            var cast = result as TDestination;

            if (cast == null)
            {
                throw new NotImplementedException();
            }

            Result = cast;

            newResult(cast);

            return cast;
        }

        protected void newResult(TDestination result)
        {
            if (NewResult != null)
            {
                NewResult(this, new NewResultEventArgs<TDestination>()
                {
                    NewResult = result,
                });
            }
        }
    }
}