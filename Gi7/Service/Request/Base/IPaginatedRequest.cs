using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gi7.Service.Request.Base
{
    public interface IPaginatedRequest<TSource, TDestination> : IGenericRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        int Page { get; set; }

        bool HasMoreItems { get; set; }

        ObservableCollection<TDestination> Result { get; set; }
        void AddResults(IEnumerable<TSource> result);
    }
}