using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Strings
{
    public static class Fnv
    {
        private const uint FNV32_PRIME = 16777619;
        private const uint FNV32_OFFSET = 0x811c9dc5;

        public static unsafe int Fnv32(string value)
        {
            unchecked
            {
                uint hash = FNV32_OFFSET;
                fixed (char* src = value)
                {
                    char* data = src;
                    while (*data != 0)
                        hash = (*data++ ^ hash) * FNV32_PRIME;
                }
                return (int)hash;
            }
        }
    }
}
