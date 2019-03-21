using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Organizer.Extensions
{
    internal static class CollectionExt
    {
        public static void Replace<TItem>(this IList<TItem> collection, TItem value1, TItem value2)
        {
            if (collection.Count > 0)
            {
                int index = collection.IndexOf(value1);
                collection.RemoveAt(index);
                collection.Insert(index, value2);
            }
            else
                throw new ArgumentException("Коллекция не содержит элементы");
        }
    }
}
