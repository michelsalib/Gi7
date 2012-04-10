using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class CollaboratorRequest : PaginatedRequest<User>
    {
        public CollaboratorRequest(string username, string repo)
        {
            Uri = string.Format("/repos/{0}/{1}/collaborators", username, repo);
        }
    }
}
