using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public static class MemberUtils
    {
        public static bool IsStatic(this PropertyInfo property)
        {
            return (property.CanRead && property.GetMethod.IsStatic) || (property.CanWrite && property.SetMethod.IsStatic);
        }
    }
}
