using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class FollowingsRequest : PaginatedRequest<User>
    {
        public FollowingsRequest(string username)
        {
            Uri = String.Format("/users/{0}/following", username);
        }
    }
}