using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmExtern
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WeakObjectAttribute : Attribute
    {
        /// <summary>
        /// Tags a method as a weak Gml import.
        /// </summary>
        public WeakObjectAttribute()
        {
        }
    }
}