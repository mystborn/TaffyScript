using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Determines which type of object a <see cref="TsObject"/> wraps.
    /// </summary>
    public enum VariableType
    {
        /// <summary>
        /// Represents a null value.
        /// </summary>
        Null,

        /// <summary>
        /// Represents a numeric value (float).
        /// </summary>
        Real,

        /// <summary>
        /// Represents a string value.
        /// </summary>
        String,

        /// <summary>
        /// Represents a TaffyScript array.
        /// </summary>
        Array,

        /// <summary>
        /// Represents a script instance.
        /// </summary>
        Delegate,

        /// <summary>
        /// Represents an instance of an object.
        /// </summary>
        Instance
    }
}
