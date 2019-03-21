using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Extensions
{
    internal static class ObservableCollectionExt
    {
        public static void RemoveRangeByIndexes<TItem>(this ObservableCollection<TItem> collection, int startIndex, int endIndex)
        {
            if (collection.Count == 0)
                throw new InvalidOperationException("Enumerable is empty");

            TItem firstElmOfStartIndex = collection.ElementAtOrDefault(startIndex);
            TItem endElmOfStartIndex = collection.ElementAtOrDefault(endIndex);

            if (firstElmOfStartIndex == null || endElmOfStartIndex == null)
                throw new IndexOutOfRangeException("");

            for(int i = startIndex; i < endIndex; i++)
            {
                collection.RemoveAt(i);
            }
        }
        public static void ReplaceItem<TItem>(this ObservableCollection<TItem> collection, TItem oldItem, TItem newItem)
        {
            int oldItemIndex = collection.IndexOf(oldItem);

            if (collection.ElementAtOrDefault(oldItemIndex) == null)
                throw new ArgumentOutOfRangeException("");

            collection.RemoveAt(oldItemIndex);
            collection.Insert(oldItemIndex, newItem);
        }
    }
}
