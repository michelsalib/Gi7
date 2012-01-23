using Gi7.Utils;

namespace Gi7.Service.Request.Base
{
    public interface IPaginatedRequest<T> : IGenericRequest<T>
        where T : new()
    {
        int Page { get; set; }

        bool HasMoreItems { get; set; }

        BetterObservableCollection<T> Result { get; set; }
        
        object CustomResult { get; set; }
    }
}