using System;
using System.ComponentModel;
using RestSharp;

namespace Gi7.Client.Request.Base
{
    public interface IRequest<TResult> : INotifyPropertyChanged
    {
        String Uri { get; }

        TResult Result { get; }
        event EventHandler ConnectionError;
        event EventHandler Unauthorized;
        event EventHandler<LoadingEventArgs> Loading;
        event EventHandler<SuccessEventArgs<TResult>> Success;

        void Execute(RestClient client, Action<TResult> callback = null);
    }
}