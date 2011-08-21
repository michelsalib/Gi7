
namespace Gi7.Service.Request.Base
{
    public interface IGithubPaginatedRequest<T> : IGithubRequest<T>
        where T : new()
    {
        int Page { get; set; }
    }
}
