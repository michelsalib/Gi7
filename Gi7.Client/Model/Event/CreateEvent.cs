using System;

namespace Gi7.Client.Model.Event
{
    public class CreateEvent : Event
    {
        public String RefType { get; set; }

        public String MasterBranch { get; set; }

        public String Ref { get; set; }

        public String Description { get; set; }
    }
}
