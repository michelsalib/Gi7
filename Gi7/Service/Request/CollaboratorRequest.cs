using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class CollaboratorRequest : PaginatedRequest<Collaborator, Collaborator>
    {
        public CollaboratorRequest(string username, string repo)
        {
            Uri = string.Format("/repos/{0}/{1}/collaborators", username, repo);
        }
    }
}
