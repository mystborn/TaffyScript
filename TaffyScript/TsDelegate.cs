using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    /// <summary>
    /// Represents a TaffyScript script.
    /// </summary>
    /// <param name="target">The target of the script.</param>
    /// <param name="args">The script arguments.</param>
    /// <returns>The scripts result.</returns>
    public delegate TsObject TsScript(TsObject[] args);

    /// <summary>
    /// Language friendly wrapper over a <see cref="TsScript"/>.
    /// </summary>
    public class TsDelegate : IEquatable<TsDelegate>
    {
        /// <summary>
        /// A wrapped TaffyScript script.
        /// </summary>
        public TsScript Script { get; private set; }

        /// <summary>
        /// The target of the wrapped script.
        /// </summary>
        public ITsInstance Target => Script.Target as ITsInstance;

        /// <summary>
        /// The name of the wrapped script.
        /// </summary>
        public string Name { get; }

        // Todo: Switch these two parameters for more efficient opcodes.

        public TsDelegate(TsScript script, string name)
        {
            Script = script;
            Name = name;
        }

        /// <summary>
        /// Invokes the wrapped script with the given arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public TsObject Invoke(params TsObject[] args)
        {
            // If the script needs a target, get it from the first index of the args array.
            // This will make it easier to invoke Delegates from TS.

            return Script(args);
        }

        public override bool Equals(object obj)
        {
            if (obj is TsDelegate del)
                return Equals(del);
            return false;
        }

        public bool Equals(TsDelegate other)
        {
            if (other is null)
                return false;
            return Script == other.Script && Target == other.Target;
        }

        public override int GetHashCode()
        {
            return Script.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(TsDelegate left, TsDelegate right)
        {
            if (!(left is null))
                return left.Equals(right);
            else return right is null;
        }

        public static bool operator !=(TsDelegate left, TsDelegate right)
        {
            if (!(left is null))
                return !left.Equals(right);
            else return !(right is null);
        }
    }
}