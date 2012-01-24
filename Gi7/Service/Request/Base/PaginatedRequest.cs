using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Gi7.Service.Request.Base
{
    public abstract class PaginatedRequest<TSource, TDestination> : ViewModelBase, IPaginatedRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        private ObservableCollection<TDestination> _result;
        private string _uri;

        public PaginatedRequest()
        {
            Page = 0;
            HasMoreItems = true;
        }

        #region IPaginatedRequest<TSource,TDestination> Members

        public int Page { get; set; }

        public bool HasMoreItems { get; set; }

        public string Uri
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
                foreach (TDestination item in cast)
                    Result.Add(item);
            else
                throw new NotImplementedException();
        }

        #endregion
    }
}