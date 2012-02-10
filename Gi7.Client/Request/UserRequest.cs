using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class UserRequest : SingleRequest<User, User>
    {
        public UserRequest(string username)
        {
            Uri = String.Format("/users/{0}", username);
        }
    }
}