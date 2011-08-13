using System;

namespace Gi7.Utils.Messages
{
    public class PanelMessage
    {
        public Type ViewType { get; private set; }

        public String Header { get; private set; }

        public PanelMessage(Type viewType, String header)
        {
            ViewType = viewType;
            Header = header;
        }
    }
}
