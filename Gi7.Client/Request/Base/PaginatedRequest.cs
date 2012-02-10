using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Gi7.Client.Utils;

namespace Gi7.Client.Request.Base
{
    public abstract class PaginatedRequest<TSource, TDestination> : ViewModelBase, IPaginatedRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        public event EventHandler<NewResultsEventArgs<TDestination>> NewResult;
        private ObservableCollection<TDestination> _result;
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

        public OverrideSettings OverrideSettings { get; protected set; }

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

        public virtual void AddResults(IEnumerable<TSource> result)
        {
            var cast = result as IEnumerable<TDestination>;
            if (cast != null)
            {
                Result.AddRange(cast);
                newResult(cast);
            }
            else
                throw new NotImplementedException();
        }

        public virtual void MoveToNextPage()
        {
            Page++;
        }

        protected void newResult(IEnumerable<TDestination> newResults)
        {
            if (NewResult != null)
            {
                NewResult(this, new NewResultsEventArgs<TDestination>()
                {
                    NewResults = newResults
                });
            }
        }
    }
}