using System.ComponentModel;

namespace Gi7.Service.Request.Base
{
    public interface IGenericRequest<TSource, TDestination> : IRequest, INotifyPropertyChanged
        where TSource : class, new()
        where TDestination : class, new() {}
}