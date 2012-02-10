using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Gi7.Client.Request.Base
{
    public interface IPaginatedRequest<TSource, TDestination> : IGenericRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        event EventHandler<NewResultsEventArgs<TDestination>> NewResult;

        int Page { get; set; }

        bool HasMoreItems { get; set; }

        ObservableCollection<TDestination> Result { get; set; }

        void AddResults(IEnumerable<TSource> result);
        
        void MoveToNextPage();
    }
}