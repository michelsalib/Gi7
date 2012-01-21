﻿using System;
using Gi7.Model;
using Gi7.Service.Request.Base;

namespace Gi7.Service.Request
{
    public class CommitCommentsRequest : PaginatedRequest<Comment>
    {
        public CommitCommentsRequest(string username, string repo, string sha)
        {
            Uri = String.Format("/repos/{0}/{1}/commits/{2}/comments", username, repo, sha);
        }
    }
}