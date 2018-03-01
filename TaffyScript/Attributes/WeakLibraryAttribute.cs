using System;

namespace TaffyScript
{
    /// <summary>
    /// Tags an assembly as a weak TaffyScript assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class WeakLibraryAttribute : Attribute
    {
        public WeakLibraryAttribute()
        {
        }
    }
}
