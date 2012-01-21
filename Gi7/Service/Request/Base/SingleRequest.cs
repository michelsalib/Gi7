using GalaSoft.MvvmLight;

namespace Gi7.Service.Request.Base
{
    public abstract class SingleRequest<T> : ViewModelBase, ISingleRequest<T>
        where T : new()
    {
        private T _result;

        #region ISingleRequest<T> Members

        public string Uri { get; protected set; }

        public OverrideSettings OverrideSettings { get; protected set; }

        public T Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        #endregion
    }
}