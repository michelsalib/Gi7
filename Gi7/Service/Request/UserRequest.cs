using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class UserRequest : SingleRequest<User>
    {
        public UserRequest(string username)
        {
            Uri = String.Format("/users/{0}", username);
        }
    }
}
