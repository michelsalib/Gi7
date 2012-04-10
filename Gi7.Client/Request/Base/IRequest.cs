using System;
using RestSharp;
using System.ComponentModel;

namespace Gi7.Client.Request.Base
{
    public interface IRequest<TResult> : INotifyPropertyChanged
    {
        event EventHandler Success;
        event EventHandler ConnectionError;
        event EventHandler Unauthorized;
        event EventHandler<LoadingEventArgs> Loading;
        event EventHandler<NewResultEventArgs<TResult>> NewResult;

        String Uri { get; }

        TResult Result { get; }

        TResult Execute(RestClient client, Action<TResult> callback = null);
    }
}