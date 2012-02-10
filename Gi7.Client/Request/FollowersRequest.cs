using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class FollowersRequest : PaginatedRequest<User, User>
    {
        public FollowersRequest(string username)
        {
            Uri = String.Format("/users/{0}/followers", username);
        }
    }
}