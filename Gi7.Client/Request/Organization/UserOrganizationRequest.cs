using System;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request.Organization
{
    public class UserOrganizationRequest : PaginatedRequest<Model.Organization>
    {
        public UserOrganizationRequest(string username)
        {
            Uri = String.Format("/users/{0}/orgs", username);
        }
    }
}