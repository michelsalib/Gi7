using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Gi7.Service.Request.Base
{
    public abstract class GithubPaginatedRequest<T> : ViewModelBase, IGithubPaginatedRequest<T>
        where T : new()
    {
        public int Page { get; set; }

        public bool HasMoreItems { get; set; }

        private string _uri;
        public string Uri
        {
            get
            {
                return String.Format("{0}?page={1}", _uri, Page);
            }
            protected set
            {
                _uri = value;
            }
        }

        public OverrideSettings OverrideSettings { get; protected set; }

        public GithubPaginatedRequest()
        {
            Page = 0;
            HasMoreItems = true;
        }

        private ObservableCollection<T> _result;
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
    }
}
