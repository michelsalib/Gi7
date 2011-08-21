using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class FollowingsRequest : GithubPaginatedRequest<User>
    {
        public FollowingsRequest(string username)
        {
            Uri = String.Format("/users/{0}/following", username);
        }
    }
}
