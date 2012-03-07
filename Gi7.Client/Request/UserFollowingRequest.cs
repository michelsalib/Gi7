using System;
using Gi7.Client.Model;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class UserFollowingRequest : SingleRequest<User, User>
    {
        public UserFollowingRequest(string username)
        {
            Uri = String.Format("/user/following/{0}", username);
        }
    }
}