using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class WeakMethodAttribute : Attribute
    {
        /// <summary>
        /// Tags a method as a weak Gml import.
        /// </summary>
        public WeakMethodAttribute()
        {
        }
    }
}
