using System;
using System.Collections.Generic;
using Gi7.Client.Model;
using Gi7.Client.Model.Event;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using Newtonsoft.Json;

namespace Gi7.Client.Utils
{
    public class EventDeserializer : IDeserializer
    {
        #region IDeserializer Members

        public string DateFormat { get; set; }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public T Deserialize<T>(RestResponse response) where T : new()
        {
            var result = new T();
            var events = result as List<Event>;

            if (events == null)
                throw new InvalidOperationException("The type must be List<Event>.");

            JArray json = JArray.Parse(response.Content);
            serializer = new JsonSerializer();

            foreach (JToken eventData in json)
            {
                try
                {
                    Event e = null;
                    var payload = eventData.SelectToken("payload");

                    switch (eventData["type"].Value<String>())
                    {
                        case "IssueCommentEvent":
                            e = new IssueCommentEvent()
                            {
                                Comment = deserialize<Comment>(payload, "comment"),
                                Issue = deserialize<Issue>(payload, "issue"),
                                Action = payload["action"].Value<String>(),
                            };
                            break;
                        case "CommitCommentEvent":
                            e = new CommitCommentEvent()
                            {
                                Comment = deserialize<Comment>(payload, "comment"),
                            };
                            break;
                        case "PushEvent":
                            e = new PushEvent
                            {
                                Head = payload["head"].Value<String>(),
                                Ref = payload["ref"].Value<String>(),
                                Size = payload["size"].Value<int>(),
                                //Commits = deserialize<List<Commit>>(payload, "commits"),
                            };
                            break;
                        case "PullRequestEvent":
                            e = new PullRequestEvent
                            {
                                Action = payload["action"].Value<String>(),
                                Number = payload["number"].Value<int>(),
                                PullRequest = deserialize<PullRequest>(payload, "pull_request"),
                            };
                            break;
                        case "IssuesEvent":
                            e = new IssuesEvent
                            {
                                Action = payload["action"].Value<String>(),
                                Issue = deserialize<Issue>(payload, "issue"),
                            };
                            break;
                        case "CreateEvent":
                            e = new CreateEvent
                            {
                                Description = payload["description"].Value<String>(),
                                Ref = payload["ref"].Value<String>(),
                                MasterBranch = payload["master_branch"].Value<String>(),
                                RefType = payload["ref_type"].Value<String>(),
                            };
                            break;
                        case "DeleteEvent":
                            e = new DeleteEvent
                            {
                                Ref = payload["ref"].Value<String>(),
                                RefType = payload["ref_type"].Value<String>(),
                            };
                            break;
                        case "WatchEvent":
                            e = new WatchEvent
                            {
                                Action = payload["action"].Value<String>()
                            };
                            break;
                        case "FollowEvent":
                            e = new FollowEvent()
                            {
                                Target = deserialize<User>(payload, "target"),
                            };
                            break;
                        case "ForkEvent":
                            e = new ForkEvent()
                            {
                                Forkee = deserialize<Repository>(payload, "forkee"),
                            };
                            break;
                        case "ForkApplyEvent":
                            e = new ForkApplyEvent()
                            {
                                Head = payload["head"].Value<String>(),
                                Before = payload["before"].Value<String>(),
                                After = payload["after"].Value<String>(),
                            };
                            break;
                        case "DownloadEvent":
                            e = new DownloadEvent()
                            {
                                Download = deserialize<Download>(payload, "download"),
                            };
                            break;
                        case "GistEvent":
                            e = new GistEvent()
                            {
                                Action = payload["action"].Value<String>(),
                                Gist = deserialize<Gist>(payload, "gist"),
                            };
                            break;
                        case "MemberEvent":
                            e = new MemberEvent()
                            {
                                Action = payload["action"].Value<String>(),
                                Member = deserialize<User>(payload, "member"),
                            };
                            break;
                        case "TeamEvent":
                            e = new TeamAddEvent()
                            {
                                Team = deserialize<Team>(payload, "team"),
                                User = deserialize<User>(payload, "user"),
                                Repo = deserialize<Repository>(payload, "repo"),
                            };
                            break;
                        case "PublicEvent":
                            e = new PublicEvent();
                            break;
                        default:
                            e = new Event();
                            break;
                    }

                    e.Actor = deserialize<User>(eventData, "actor");
                    e.Public = eventData["public"].Value<bool?>();
                    e.CreatedAt = eventData["created_at"].Value<DateTime>();
                    e.Repo = deserialize<Repository>(eventData, "repo");

                    events.Add(e);
                }
                catch (Exception)
                {
                    // might silencly fail on an event
                }
            }

            return result;
        }

        #endregion

        private JsonSerializer serializer;

        private T deserialize<T>(JToken token, String member) where T : new()
        {
            try
            {
                return serializer.Deserialize<T>(token.SelectToken(member).CreateReader());
            }
            catch (Exception)
            {
                return new T();
            }
        }
    }
}