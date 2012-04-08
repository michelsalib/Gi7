using RestSharp;
using System;
namespace Gi7.Client.Request.Base
{
    public interface ISingleRequest<TSource, TDestination> : IGenericRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        event EventHandler<NewResultEventArgs<TDestination>> NewResult;

        TDestination Result { get; set; }

        TDestination SetResult(TSource result);

        TDestination Execute(RestClient client, Action<TDestination> callback = null);
    }
}