using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Gi7.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Transforms a IEnumerable to an ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            ObservableCollection<T> observableCollection = new ObservableCollection<T>();
            foreach (T item in collection)
            {
                observableCollection.Add(item);
            }

            return observableCollection;
        }

        /// <summary>
        /// Trunk a date to its day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime Trunk(this DateTime date)
        {
            return date.Subtract(date.TimeOfDay);
        }
    }
}
