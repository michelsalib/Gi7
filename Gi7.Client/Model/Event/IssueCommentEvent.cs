using System;

namespace Gi7.Client.Model.Event
{
    public class IssueCommentEvent : Event
    {
        public String Action { get; set; }

        public Issue Issue { get; set; }

        public Comment Comment { get; set; }
    }
}
