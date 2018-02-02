using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmParser
{
    public static class Extensions
    {
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int n)
        {
            var iter = source.GetEnumerator();
            bool hasItems = false;
            var cache = new Queue<T>(n + 1);

            do
            {
                hasItems = iter.MoveNext();
                if (hasItems)
                {
                    cache.Enqueue(iter.Current);
                    if (cache.Count > n)
                        yield return cache.Dequeue();
                }
            }
            while (hasItems);
        }
    }
}
