using System;

namespace Gi7.Client.Model.Event
{
    public class GistEvent : Event
    {
        public String Action { get; set; }

        public Gist Gist { get; set; }
    }
}
