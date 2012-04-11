using System;
using System.ComponentModel;
using RestSharp;

namespace Gi7.Client.Request.Base
{
    public interface IRequest<TResult> : INotifyPropertyChanged
    {
        event EventHandler ConnectionError;
        event EventHandler Unauthorized;
        event EventHandler<LoadingEventArgs> Loading;
        event EventHandler<SuccessEventArgs<TResult>> Success;

        String Uri { get; }

        TResult Result { get; }

        void Execute(RestClient client, Action<TResult> callback = null);
    }
}