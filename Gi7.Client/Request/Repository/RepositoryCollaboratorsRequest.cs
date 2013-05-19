using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class RepositoryCollaboratorsRequest : PaginatedRequest<User>
    {
        public RepositoryCollaboratorsRequest(string username, string repo)
        {
            Uri = string.Format("/repos/{0}/{1}/collaborators", username, repo);
        }
    }
}