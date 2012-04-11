using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Gi7.Client.Model;
using Gi7.Client.Model.Extra;
using Gi7.Client.Request.Base;
using Gi7.Client.Utils;
using RestSharp;

namespace Gi7.Client.Request.Commit
{
    public class List : PaginatedRequest<PushGroup>
    {
        private readonly string branch;

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

        public List(string username, string repo, string branch)
        {
            this.branch = branch;
            Uri = String.Format("/repos/{0}/{1}/commits", username, repo);
        }

        public override void Execute(RestClient client, Action<ObservableCollection<PushGroup>> callback = null)
        {
            if (Result == null)
            {
                Result = new ObservableCollection<PushGroup>();
            }

            var request = new RestRequest(Uri);

            preRequest(client, request);

            RaiseLoading(true);

            client.ExecuteAsync<List<Push>>(request, r =>
            {
                if (Result == null)
                {
                    Result = new ObservableCollection<PushGroup>();
                }

                RaiseLoading(false);

                if (r.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RaiseUnauthorized();
                }
                else if (r.ResponseStatus == ResponseStatus.Error)
                {
                    RaiseConnectionError();
                }
                else
                {
                    Page++;

                    if (r.Data.Count < 30)
                    {
                        HasMoreItems = false;
                    }

                    var groupedResult = r.Data.GroupBy(i => i.Commit.Commiter ? i.Commit.Commiter.Date.Trunk() : i.Commit.Author.Date.Trunk());

                    var pushGroupResult = new ObservableCollection<PushGroup>();

                    foreach (var group in groupedResult)
                    {
                        var existingGroup = Result.FirstOrDefault(g => g.Date == group.Key);
                        if (existingGroup == null)
                        {
                            existingGroup = new PushGroup { Date = group.Key };
                            Result.Add(existingGroup);
                            pushGroupResult.Add(existingGroup);

                        }
                        existingGroup.AddRange(group);
                    }

                    RaiseSuccess(pushGroupResult);

                    if (callback != null)
                    {
                        callback(pushGroupResult);
                    }
                }
            });
        }
    }
}