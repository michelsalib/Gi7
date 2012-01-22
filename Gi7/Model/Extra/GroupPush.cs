using System;
using System.Collections.Generic;
using System.Linq;
using Gi7.Utils;

namespace Gi7.Model.Extra
{
    public class GroupPush
    {
        public DateTime Date { get; set; }
        public BetterObservableCollection<Push> Pushes { get; set; }

        public GroupPush()
        {
            Pushes = new BetterObservableCollection<Push>();
        }

        public GroupPush(DateTime date, BetterObservableCollection<Push> pushes)
        {
            Date = date;
            Pushes = pushes;
        }

        public GroupPush(DateTime date, IEnumerable<Push> pushes)
        {
            Date = date;
            Pushes = new BetterObservableCollection<Push>(pushes);
        }
    }
}
