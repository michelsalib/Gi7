using System;
using System.Xml.Serialization;

namespace Gi7.Client.Model.Event
{
    [XmlInclude(typeof(CommitCommentEvent)), XmlInclude(typeof(CreateEvent)), XmlInclude(typeof(DeleteEvent)), XmlInclude(typeof(DownloadEvent)), XmlInclude(typeof(FollowEvent)), XmlInclude(typeof(ForkApplyEvent)), XmlInclude(typeof(ForkEvent)), XmlInclude(typeof(GistEvent)), XmlInclude(typeof(IssueCommentEvent)), XmlInclude(typeof(IssuesEvent)), XmlInclude(typeof(MemberEvent)), XmlInclude(typeof(PublicEvent)), XmlInclude(typeof(PullRequestEvent)), XmlInclude(typeof(PushEvent)), XmlInclude(typeof(TeamAddEvent)), XmlInclude(typeof(WatchEvent))]
    public class Event
    {
        public DateTime CreatedAt { get; set; }

        public User Actor { get; set; }

        public Repository Repo { get; set; }

        public bool? Public { get; set; }
    }
}
