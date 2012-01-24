using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class FollowersRequest : PaginatedRequest<User, User>
    {
        public FollowersRequest(string username)
        {
            Uri = String.Format("/users/{0}/followers", username);
        }
    }
}