
namespace Gi7.Service.Request.Base
{
    public interface IGithubSingleRequest<T> : IGithubRequest<T>
        where T : new()
    {
        T Result { get; set; }
    }
}
