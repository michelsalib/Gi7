using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Gi7.Client.Utils;
using RestSharp;
using System.Net;

namespace Gi7.Client.Request.Base
{
    public abstract class PaginatedRequest<TSource, TDestination> : ViewModelBase, IPaginatedRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        public event EventHandler Success;
        public event EventHandler ConnectionError;
        public event EventHandler Unauthorized;
        public event EventHandler<LoadingEventArgs> Loading;
        public event EventHandler<NewResultEventArgs<IEnumerable<TDestination>>> NewResult;
        private ObservableCollection<TDestination> _result = new ObservableCollection<TDestination>();
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

        public ObservableCollection<TDestination> Result
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

        public virtual ObservableCollection<TDestination> Execute(RestClient client, Action<IEnumerable<TDestination>> callback = null)
        {
            var request = new RestRequest(Uri);

            preRequest(client, request);

            if (Loading != null)
            {
                Loading(this, new LoadingEventArgs(true));
            }

            client.ExecuteAsync<List<TSource>>(request, r =>
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

                    Page++;

                    if (r.Data.Count < 30)
                    {
                        HasMoreItems = false;
                    }

                    var cast = AddResults(r.Data);

                    if (callback != null)
                        callback(cast);
                }
            });

            return Result;
        }

        protected virtual void preRequest(RestClient client, RestRequest request)
        {

        }

        public virtual IEnumerable<TDestination> AddResults(IEnumerable<TSource> result)
        {
            var cast = result as IEnumerable<TDestination>;

            if (cast == null) {
                throw new NotImplementedException();
            }

            Result.AddRange(cast);

            newResult(cast);

            return cast;
        }

        protected void newResult(IEnumerable<TDestination> result)
        {
            if (NewResult != null)
            {
                NewResult(this, new NewResultEventArgs<IEnumerable<TDestination>>()
                {
                    NewResult = result,
                });
            }
        }
    }
}