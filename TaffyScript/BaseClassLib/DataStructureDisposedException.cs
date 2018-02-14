using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public class DataStructureDestroyedException : Exception
    {
        public string DsType { get; }
        public int Index { get; }

        public DataStructureDestroyedException(string dsType, int index)
            : base($"The {dsType} data structure at index {index} was already destroyed.")
        {
            DsType = dsType;
            Index = index;
        }
    }
}
