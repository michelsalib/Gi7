using System.ComponentModel;

namespace Gi7.Client.Request.Base
{
    public interface IGenericRequest<TSource, TDestination> : IRequest, INotifyPropertyChanged
        where TSource : class, new()
        where TDestination : class, new()
    {
    }
}