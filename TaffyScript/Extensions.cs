using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    public static class Extensions
    {
        public static void Shuffle(Array array)
        {
            int n = array.Length;
            while(n > 1)
            {
                var k = Bcl.Rng.Next(n--);
                var value = array.GetValue(k);
                array.SetValue(array.GetValue(n), k);
                array.SetValue(value, n);
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while(n > 1)
            {
                var k = Bcl.Rng.Next(n--);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
