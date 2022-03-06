using System.Collections.Generic;

namespace UsefulMethods.ExtensionMethods
{
    public static class IsBetweenExtension
    {
        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }
    }
}
