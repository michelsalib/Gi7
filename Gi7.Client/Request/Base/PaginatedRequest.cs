using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Gi7.Client.Utils;
using RestSharp;
using System.Net;

namespace Gi7.Client.Request.Base
{
    public abstract class PaginatedRequest<TResult> : ViewModelBase, IPaginatedRequest<TResult>
        where TResult : class, new()
    {
        public event EventHandler Success;
        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;
        public event EventHandler<LoadingEventArgs> Loading;
        public event EventHandler<NewResultEventArgs<ObservableCollection<TResult>>> NewResult;
        private ObservableCollection<TResult> _result = new ObservableCollection<TResult>();
        protected string _uri;

        public PaginatedRequest()
        {
            Page = 1;
            HasMoreItems = true;
        }

        public int Page { get; set; }

        public bool HasMoreItems { get; set; }

        public virtual string Uri
        {
            get { return String.Format("{0}?page={1}", _uri, Page); }
            protected set { _uri = value; }
        }

        public ObservableCollection<TResult> Result
        {
            get { return _result; }
            set
            {
                if (value != _result)
                {
                    _result = value;
                    RaisePropertyChanged("Result");
                }
            }
        }

        public virtual ObservableCollection<TResult> Execute(RestClient client, Action<ObservableCollection<TResult>> callback = null)
        {
            var request = new RestRequest(Uri);

            preRequest(client, request);

            RaiseLoading(true);

            client.ExecuteAsync<List<TResult>>(request, r =>
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
                    RaiseSuccess();

                    Page++;

                    if (r.Data.Count < 30)
                    {
                        HasMoreItems = false;
                    }

                    Result.AddRange(r.Data);

                    RaiseNewResult(new ObservableCollection<TResult>(r.Data));

                    if (callback != null)
                    {
                        callback(new ObservableCollection<TResult>(r.Data));
                    }
                }
            });

            return Result;
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

        protected void RaiseSuccess()
        {
            if (Success != null)
            {
                Success(this, new EventArgs());
            }
        }

        protected void RaiseNewResult(ObservableCollection<TResult> result)
        {
            if (NewResult != null)
            {
                NewResult(this, new NewResultEventArgs<ObservableCollection<TResult>>()
                {
                    NewResult = result,
                });
            }
        }
    }
}