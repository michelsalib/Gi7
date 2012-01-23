using System;
using System.Collections.Generic;

namespace Gi7.Utils
{
    public static class Extensions
    {
        public static DateTime Trunk(this DateTime date)
        {
            return date.Subtract(date.TimeOfDay);
        }

        public static BetterObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new BetterObservableCollection<T>(enumerable);
        }
    }
}
