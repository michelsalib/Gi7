using System.Collections.ObjectModel;

namespace Gi7.Client.Request.Base
{
    public interface IPaginatedRequest<TResult> : IRequest<ObservableCollection<TResult>>
        where TResult : class, new()
    {
        int Page { get; set; }

        bool HasMoreItems { get; set; }
    }
}