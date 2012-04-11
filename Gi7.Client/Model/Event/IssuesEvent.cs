using System;

namespace Gi7.Client.Model.Event
{
    public class IssuesEvent : Event
    {
        public String Action { get; set; }

        public Issue Issue { get; set; }
    }
}
