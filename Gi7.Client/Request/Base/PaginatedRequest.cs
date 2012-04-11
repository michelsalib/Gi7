using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Gi7.Client.Utils;
using RestSharp;
using System.Net;

namespace Gi7.Client.Request.Base
{
    public abstract class PaginatedRequest<TResult> : SingleRequest<ObservableCollection<TResult>>, IPaginatedRequest<TResult>
        where TResult : class, new()
    {
        private ObservableCollection<TResult> _result = new ObservableCollection<TResult>();
        protected string _uri;
        private int _page = 1;
        private bool _hasMoreItems = true;

        public bool HasMoreItems
        {
            get { return _hasMoreItems ; }
            set { _hasMoreItems = value; }
        }
        
        public int Page
        {
            get { return _page; }
            set { _page = value; }
        }
        

        public override string Uri
        {
            get { return String.Format("{0}?page={1}", _uri, Page); }
            protected set { _uri = value; }
        }

        public override void Execute(RestClient client, Action<ObservableCollection<TResult>> callback = null)
        {
            if (Result == null)
            {
                Result = new ObservableCollection<TResult>();
            }

            var request = new RestRequest(Uri);

            preRequest(client, request);

            RaiseLoading(true);

            client.ExecuteAsync<List<TResult>>(request, r =>
            {
                if (Result == null)
                {
                    Result = new ObservableCollection<TResult>();
                }

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
                    Page++;

                    if (r.Data.Count < 30)
                    {
                        HasMoreItems = false;
                    }

                    Result.AddRange(r.Data);

                    RaiseSuccess(new ObservableCollection<TResult>(r.Data));

                    if (callback != null)
                    {
                        callback(new ObservableCollection<TResult>(r.Data));
                    }
                }
            });
        }

    }
}