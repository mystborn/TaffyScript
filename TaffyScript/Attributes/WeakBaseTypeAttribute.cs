using System;

namespace TaffyScript
{
    /// <summary>
    /// Tags a type as a TaffyScript base type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class TaffyScriptBaseTypeAttribute : Attribute
    {
        public TaffyScriptBaseTypeAttribute()
        {
        }
    }
}
