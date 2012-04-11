using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Repository
{
    public class ListCollaborators : PaginatedRequest<Model.User>
    {
        public ListCollaborators(string username, string repo)
        {
            Uri = string.Format("/repos/{0}/{1}/collaborators", username, repo);
        }
    }
}
