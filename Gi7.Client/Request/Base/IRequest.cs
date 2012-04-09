using System;

namespace Gi7.Client.Request.Base
{
    public interface IRequest
    {
        event EventHandler Success;
        event EventHandler ConnectionError;
        event EventHandler Unauthorized;
        event EventHandler<LoadingEventArgs> Loading;

        String Uri { get; }
    }
}