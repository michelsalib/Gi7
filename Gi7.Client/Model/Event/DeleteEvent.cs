using System;

namespace Gi7.Client.Model.Event
{
    public class DeleteEvent : Event
    {
        public String RefType { get; set; }

        public String Ref { get; set; }
    }
}
