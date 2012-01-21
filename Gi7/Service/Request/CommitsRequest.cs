﻿using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class CommitsRequest : PaginatedRequest<Push>
    {
        public CommitsRequest(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/commits", username, repo);
        }
    }
}