using System;

namespace Gi7.Client.Model.Event
{
    public class PushEvent : Event
    {
        public String Head { get; set; }

        public String Ref { get; set; }

        public int Size { get; set; }

        //public List<Commit> Commits { get; set; }
    }
}
