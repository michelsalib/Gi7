using System;

namespace Gi7.Client.Model.Event
{
    public class MemberEvent : Event
    {
        public User Member { get; set; }

        public String Action { get; set; }
    }
}
