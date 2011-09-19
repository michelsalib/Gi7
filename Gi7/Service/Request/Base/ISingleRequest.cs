
namespace Gi7.Service.Request.Base
{
    public interface ISingleRequest<T> : IGenericRequest<T>
        where T : new()
    {
        T Result { get; set; }
    }
}
