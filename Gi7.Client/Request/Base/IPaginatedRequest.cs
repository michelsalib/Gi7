using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using RestSharp;

namespace Gi7.Client.Request.Base
{
    public interface IPaginatedRequest<TSource, TDestination> : IGenericRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        event EventHandler<NewResultEventArgs<IEnumerable<TDestination>>> NewResult;

        int Page { get; set; }

        bool HasMoreItems { get; set; }

        ObservableCollection<TDestination> Result { get; set; }

        IEnumerable<TDestination> AddResults(IEnumerable<TSource> result);

        ObservableCollection<TDestination> Execute(RestClient client, Action<IEnumerable<TDestination>> callback = null);
    }
}