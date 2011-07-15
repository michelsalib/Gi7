using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Github7.Model.Feed
{
    [XmlInclude(typeof(IssueCommentFeed))]
    [XmlInclude(typeof(CommitCommentFeed))]
    [XmlInclude(typeof(PullRequestFeed))]
    [XmlInclude(typeof(IssueFeed))]
    [XmlInclude(typeof(PushFeed))]
    [XmlInclude(typeof(CreateFeed))]
    [XmlInclude(typeof(WatchFeed))]
    [XmlInclude(typeof(DeleteFeed))]
    public class Feed
    {
        public Repository Repository { get; set; }

        public User User { get; set; }

        public DateTime CreatedAt { get; set; }

        public String Actor { get; set; }

        public String Url { get; set; }

        public bool Public { get; set; }

        public virtual String Template
        {
            get
            {
                return "did a non supported action at";
            }
        }

        public virtual String Image
        {
            get
            {
                return null;
            }
        }
    }
}
