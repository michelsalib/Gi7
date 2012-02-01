using System;
using System.Collections.Generic;
using System.Linq;
using Gi7.Model;
using Gi7.Model.Extra;
using Gi7.Service.Request.Base;
using Gi7.Utils;

namespace Gi7.Service.Request
{
    public class CommitsRequest : PaginatedRequest<Push, PushGroup>
    {
        private readonly string branch;

        public CommitsRequest(string username, string repo, string branch = "master")
        {
            this.branch = branch;
            Uri = String.Format("/repos/{0}/{1}/commits?sha={2}", username, repo, branch);
        }

        public override void AddResults(IEnumerable<Push> result)
        {
            var groupedResult = result.GroupBy(i => i.Commit.Author.Date.Trunk());
            foreach (var group in groupedResult)
            {
                var existingGroup = Result.FirstOrDefault(g => g.Date == group.Key);
                if (existingGroup == null)
                {
                    existingGroup = new PushGroup
                    {
                        Date = group.Key
                    };
                    Result.Add(existingGroup);
                }
                existingGroup.AddRange(group);
            }
        }

        public override string Uri
        {
            get {
                var lastGroup = Result.LastOrDefault();
                return lastGroup != null ?
                    String.Format("{0}?last_sha={1}&sha={2}", _uri, lastGroup.Last().Sha, branch) : 
                    _uri;
            }
            protected set { base.Uri = value; }
        }
    }
}