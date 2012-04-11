using System;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.User
{
    public class ListFollowers : PaginatedRequest<Model.User>
    {
        public ListFollowers(string username)
        {
            Uri = String.Format("/users/{0}/followers", username);
        }
    }
}