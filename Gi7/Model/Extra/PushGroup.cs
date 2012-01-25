using System;
using System.Collections.ObjectModel;

namespace Gi7.Model.Extra
{
    public class PushGroup : ObservableCollection<Push>
    {
        public DateTime Date { get; set; }
    }
}