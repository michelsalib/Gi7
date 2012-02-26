using System;

namespace Gi7.Client.Model.Event
{
    public class ForkApplyEvent : Event
    {
        public String Head { get; set; }

        public String Before { get; set; }

        public String After { get; set; }
    }
}
