using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Repository
{
    public class List : PaginatedRequest<Model.Repository>
    {
        public List()
        {
            Uri = "/user/repos";
        }
    }
}
