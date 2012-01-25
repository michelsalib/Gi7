using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            var observableCollection = new ObservableCollection<T>();
            observableCollection.AddRange(collection);

            return observableCollection;
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="itemsToAdd"> The collection whose elements should be added to the end of the List Of T. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
                list.Add(item);
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