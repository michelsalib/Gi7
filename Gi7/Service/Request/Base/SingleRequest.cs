using GalaSoft.MvvmLight;
using System;

namespace Gi7.Service.Request.Base
{
    public abstract class SingleRequest<TSource, TDestination> : ViewModelBase, ISingleRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        private TDestination _result;

        #region ISingleRequest<TSource, TDestination> Members

        public string Uri { get; protected set; }

        public OverrideSettings OverrideSettings { get; protected set; }

        public TDestination Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        public void SetResult(TSource result)
        {
            var cast = result as TDestination;
            if (cast != null)
            {
                Result = cast as TDestination;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion

    }
}