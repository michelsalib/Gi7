using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gi7.Client.Model;
using Gi7.Client.Model.Extra;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;

namespace Gi7.Client.Request
{
    public class CommitsRequest : PaginatedRequest<Push, PushGroup>
    {
        private readonly string branch;

        public CommitsRequest(string username, string repo, string branch)
        {
            this.branch = branch;
            Uri = String.Format("/repos/{0}/{1}/commits", username, repo);
        }

        public override IEnumerable<PushGroup> AddResults(IEnumerable<Push> result)
        {
            var groupedResult = result.GroupBy(i => i.Commit.Commiter ? i.Commit.Commiter.Date.Trunk() : i.Commit.Author.Date.Trunk());

            var r = new List<PushGroup>();

            foreach (var group in groupedResult)
            {
                var existingGroup = Result.FirstOrDefault(g => g.Date == group.Key);
                if (existingGroup == null)
                {
                    existingGroup = new PushGroup { Date = group.Key };
                    Result.Add(existingGroup);
                    r.Add(existingGroup);

                }
                existingGroup.AddRange(group);
            }

            newResult(r);

            return r;
        }
        
        public override string Uri
        {
            get
            {
                PushGroup lastGroup;
                if (lastGroup = Result.LastOrDefault())
                {
                    return String.Format("{0}?last_sha={1}", _uri, lastGroup.Last().Sha);
                }
                return String.Format("{0}?sha={1}", _uri, branch);
            }
            protected set { _uri = value; }
        }
    }
}