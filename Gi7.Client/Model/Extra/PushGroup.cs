using System;
using System.Collections.ObjectModel;

namespace Gi7.Client.Model.Extra
{
    public class PushGroup : ObservableCollection<Push>
    {
        public DateTime Date { get; set; }

        public static implicit operator bool(PushGroup p)
        {
            return p != null;
        }
    }
}
