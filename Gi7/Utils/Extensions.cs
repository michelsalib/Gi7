using System;

namespace Gi7.Utils
{
    public static class Extensions
    {
        public static DateTime Trunk(this DateTime date)
        {
            return date.Subtract(date.TimeOfDay);
        }
    }
}
