using System.Collections.ObjectModel;
using Gi7.Utils;
using System.Collections.Generic;

namespace Gi7.Service.Request.Base
{
    public interface IPaginatedRequest<TSource, TDestination> : IGenericRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        int Page { get; set; }

        bool HasMoreItems { get; set; }

        void AddResults(IEnumerable<TSource> result);

        ObservableCollection<TDestination> Result { get; set; }
    }
}