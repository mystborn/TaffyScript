using System;

namespace TaffyScript
{
    /// <summary>
    /// Tags a method as a weak TaffyScript method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TaffyScriptMethodAttribute : Attribute
    {
        public TaffyScriptMethodAttribute()
        {
        }
    }
}
