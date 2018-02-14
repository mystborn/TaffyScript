using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class WeakLibraryAttribute : Attribute
    {
        /// <summary>
        /// Tags an assembly as a weak Gml library
        /// </summary>
        public WeakLibraryAttribute()
        {
        }
    }
}
