
using System.Collections.ObjectModel;
namespace Gi7.Service.Request.Base
{
    public interface IPaginatedRequest<T> : IGenericRequest<T>
        where T : new()
    {
        int Page { get; set; }

        bool HasMoreItems { get; set; }

        ObservableCollection<T> Result { get; set; }
    }
}
