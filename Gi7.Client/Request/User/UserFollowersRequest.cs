using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class UserFollowersRequest : PaginatedRequest<User>
    {
        public UserFollowersRequest(string username)
        {
            Uri = String.Format("/users/{0}/followers", username);
        }
    }
}