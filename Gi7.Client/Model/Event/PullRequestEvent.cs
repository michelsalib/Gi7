using System;

namespace Gi7.Client.Model.Event
{
    public class PullRequestEvent : Event
    {
        public String Action { get; set; }

        public int Number { get; set; }

        public PullRequest PullRequest { get; set; }
    }
}
