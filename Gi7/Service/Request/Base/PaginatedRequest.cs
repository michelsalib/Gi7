using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Gi7.Service.Request.Base
{
    public abstract class PaginatedRequest<T> : ViewModelBase, IPaginatedRequest<T>
        where T : new()
    {
        private ObservableCollection<T> _result;
        private string _uri;

        public PaginatedRequest()
        {
            Page = 0;
            HasMoreItems = true;
        }

        #region IPaginatedRequest<T> Members

        public int Page { get; set; }

        public bool HasMoreItems { get; set; }

        public string Uri
        {
            get { return String.Format("{0}?page={1}", _uri, Page); }
            protected set { _uri = value; }
        }

        public OverrideSettings OverrideSettings { get; protected set; }

        public ObservableCollection<T> Result
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

        #endregion
    }
}