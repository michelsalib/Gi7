using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gi7.Utils
{
    public class BetterObservableCollection<T> : ObservableCollection<T>
    {
        public BetterObservableCollection()
        {
            
        }

        public BetterObservableCollection(List<T> list)
            : base(list)
        {
        }
        
        public BetterObservableCollection(IEnumerable<T> list)
            : base(list)
        {
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }
    }
}
