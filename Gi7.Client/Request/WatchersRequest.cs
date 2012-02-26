using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class WatchersRequest : PaginatedRequest<User, User>
    {
        public WatchersRequest(string username, string repo)
        {
            Uri = string.Format("/repos/{0}/{1}/watchers", username, repo);
        }
    }
}
