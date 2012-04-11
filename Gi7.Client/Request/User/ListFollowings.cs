using System;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.User
{
    public class ListFollowings : PaginatedRequest<Model.User>
    {
        public ListFollowings(string username)
        {
            Uri = String.Format("/users/{0}/following", username);
        }
    }
}