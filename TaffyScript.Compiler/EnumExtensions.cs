using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Compiler
{
    public static class EnumExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AccessModifiers And(this AccessModifiers left, AccessModifiers right)
        {
            return left & right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFlag(this AccessModifiers left, AccessModifiers right)
        {
            return (left & right) == right;
        }
    }
}
