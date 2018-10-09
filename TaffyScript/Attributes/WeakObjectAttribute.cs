using System;

namespace TaffyScript
{
    /// <summary>
    /// Tags an object as a weak TaffyScript object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TaffyScriptObjectAttribute : Attribute
    {
        public string Name { get; }

        public TaffyScriptObjectAttribute()
        {
            Name = null;
        }

        public TaffyScriptObjectAttribute(string name)
        {
            Name = name;
        }
    }
}