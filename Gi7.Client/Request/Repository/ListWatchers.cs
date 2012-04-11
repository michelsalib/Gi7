using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Repository
{
    public class ListWatchers : PaginatedRequest<Model.User>
    {
        public ListWatchers(string username, string repo)
        {
            Uri = string.Format("/repos/{0}/{1}/watchers", username, repo);
        }
    }
}
