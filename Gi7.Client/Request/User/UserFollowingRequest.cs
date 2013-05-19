using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class UserFollowingRequest : PaginatedRequest<User>
    {
        public UserFollowingRequest(string username)
        {
            Uri = String.Format("/users/{0}/following", username);
        }
    }
}