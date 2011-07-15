using System;
using System.Collections.Generic;
using System.Globalization;
using Github7.Model;
using Github7.Model.Feed;
using Newtonsoft.Json.Linq;
using RestSharp.Deserializers;
using RestSharp.Extensions;

namespace Github7.Utils
{
    public class FeedDeserializer : IDeserializer
    {
        public string DateFormat { get; set; }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public T Deserialize<T>(RestSharp.RestResponse response) where T: new()
        {
            T result = new T();
            var feeds = result as List<Feed>;

            if (feeds == null)
                throw new InvalidOperationException("The type must be List<Feed>.");

            var json = JArray.Parse(response.Content);
            var defaultDeserializer = new JsonDeserializer();

            foreach(var feedData in json){
                Feed feed = null;

                switch (feedData["type"].Value<String>())
                {
                    case "IssueCommentEvent":
                        feed = new IssueCommentFeed()
                        {
                            CommentId = feedData["payload"]["comment_id"].Value<int>(),
                            IssueId = feedData["payload"]["issue_id"].Value<int>()
                        };
                        break;
                    case "CommitCommentEvent":
                        feed = new CommitCommentFeed()
                        {
                            CommentId = feedData["payload"]["comment_id"].Value<int>(),
                            Commit = feedData["payload"]["commit"].Value<String>()
                        };
                        break;
                    case "PushEvent":
                        feed = new PushFeed()
                        {
                            Head = feedData["payload"]["head"].Value<String>(),
                            Ref = feedData["payload"]["ref"].Value<String>(),
                            Size = feedData["payload"]["size"].Value<int>(),
                        };
                        break;
                    case "PullRequestEvent":
                        feed = new PullRequestFeed()
                        {
                            Action = feedData["payload"]["action"].Value<String>(),
                            Number = feedData["payload"]["number"].Value<int>(),
                            PullRequest = new PullRequest()
                            {
                                Additions = feedData["payload"]["pull_request"]["additions"].Value<int>(),
                                Commits = feedData["payload"]["pull_request"]["commits"].Value<int>(),
                                Deletions = feedData["payload"]["pull_request"]["deletions"].Value<int>(),
                                Id = feedData["payload"]["pull_request"]["id"].Value<int>(),
                                IssueId = feedData["payload"]["pull_request"]["issue_id"].Value<int>(),
                                Number = feedData["payload"]["pull_request"]["number"].Value<int>(),
                                Title = feedData["payload"]["pull_request"]["title"].Value<String>(),
                            }
                        };
                        break;
                    case "IssuesEvent":
                        feed = new IssueFeed()
                        {
                            Action = feedData["payload"]["action"].Value<String>(),
                            Issue = feedData["payload"]["issue"].Value<int>(),
                            Number = feedData["payload"]["number"].Value<int>()
                        };
                        break;
                    case "CreateEvent":
                        feed = new CreateFeed()
                        {
                            Description = feedData["payload"]["description"].Value<String>(),
                            Ref = feedData["payload"]["ref"].Value<String>(),
                            MasterBranch = feedData["payload"]["master_branch"].Value<String>(),
                            RefType = feedData["payload"]["ref_type"].Value<String>(),
                        };
                        break;
                    case "DeleteEvent":
                        feed = new DeleteFeed()
                        {
                            Ref = feedData["payload"]["ref"].Value<String>(),
                            RefType = feedData["payload"]["ref_type"].Value<String>(),
                        };
                        break;
                    case "WatchEvent":
                        feed = new WatchFeed()
                        {
                            Action = feedData["payload"]["action"].Value<String>()
                        };
                        break;
                    default:
                        feed = new Feed();
                        break;
                }

                feed.Actor = feedData["actor"].Value<String>();
                feed.Public = feedData["public"].Value<bool>();
                feed.CreatedAt = feedData["created_at"].Value<DateTime>();
                feed.Url = feedData["url"].Value<String>();

                feed.Repository = new Repository()
                {
                    Name = feedData["repository"]["name"].Value<String>(),
                    Url = feedData["repository"]["url"].Value<String>(),
                    Owner = new User(){
                        Login = feedData["repository"]["owner"].Value<String>()
                    }
                };

                feed.User = new User()
                {
                    Login = feedData["actor_attributes"]["login"].Value<String>(),
                    AvatarUrl = "https://secure.gravatar.com/avatar/" + feedData["actor_attributes"]["gravatar_id"].Value<String>(),
                };

                feeds.Add(feed);
            }

            return result;
        }
    }
}
