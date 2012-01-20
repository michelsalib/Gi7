using System;

namespace Gi7.Service
{
    public class LoadingEventArgs : EventArgs
    {
        public LoadingEventArgs(bool isLoading)
        {
            IsLoading = isLoading;
        }

        public bool IsLoading { get; private set; }
    }
}