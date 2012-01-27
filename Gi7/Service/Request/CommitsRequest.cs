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
        public CommitsRequest(string username, string repo)
        {
            Uri = String.Format("/repos/{0}/{1}/commits", username, repo);
        }

        public override void AddResults(IEnumerable<Push> result)
        {
            IEnumerable<IGrouping<DateTime, Push>> groupedResult = result.GroupBy(i => i.Commit.Author.Date.Trunk());
            foreach (var group in groupedResult)
            {
                PushGroup existingGroup = Result.FirstOrDefault(g => g.Date == group.Key);
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
                if (lastGroup != null)
                {
                    return String.Format("{0}?last_sha={1}", _uri, lastGroup.Last().Sha);
                }
                else
                {
                    return _uri;
                }
            }
            protected set { base.Uri = value; }
        }
    }
}