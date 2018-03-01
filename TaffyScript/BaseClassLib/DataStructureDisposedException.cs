using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Exception thrown when trying to access a data structure that no longer exists.
    /// </summary>
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
