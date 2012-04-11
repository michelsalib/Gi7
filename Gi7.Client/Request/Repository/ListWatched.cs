using System;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Repository
{
    public class ListWatched : PaginatedRequest<Model.Repository>
    {
        public ListWatched(string username)
        {
            Uri = String.Format("/users/{0}/watched", username);
        }
    }
}