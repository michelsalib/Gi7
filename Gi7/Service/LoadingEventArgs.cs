using System;

namespace Gi7.Service
{
    public class LoadingEventArgs : EventArgs
    {
        public bool IsLoading { get; private set; }

        public LoadingEventArgs(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }
}
