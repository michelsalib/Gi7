using System;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.User
{
    public class Get : SingleRequest<Model.User>
    {
        public Get(string username)
        {
            Uri = String.Format("/users/{0}", username);
        }
    }
}