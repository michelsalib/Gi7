using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class RepositoriesRequest : PaginatedRequest<Repository>
    {
        public RepositoriesRequest()
        {
            Uri = "/user/repos";
        }
    }
}