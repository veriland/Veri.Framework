using System;
using System.Collections.Generic;
using System.Text;

namespace Veri.System.Extensions
{
    public static class ListExtensions
    {
        public static void AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
    }
}
