using System;

namespace TaffyScript
{
    /// <summary>
    /// Tags an object as a weak TaffyScript object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TaffyScriptObjectAttribute : Attribute
    {
        public TaffyScriptObjectAttribute()
        {
        }
    }
}