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
    public class TsDelegate : TsObject
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

        public override VariableType Type => VariableType.Delegate;
        public override object WeakValue => Script;

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

        public override TsDelegate GetDelegate()
        {
            return this;
        }

        public override TsObject[] GetArray() => throw new InvalidTsTypeException($"Variable is supposed to be of type Array, is {Type} instead.");
        public override float GetNumber() => throw new InvalidTsTypeException($"Variable is supposed to be of type Number, is {Type} instead.");
        public override ITsInstance GetInstance() => throw new InvalidTsTypeException($"Variable is supposed to be of type Instance, is {Type} instead.");
        public override string GetString() => throw new InvalidTsTypeException($"Variable is supposed to be of type String, is {Type} instead.");

        public override bool Equals(object obj)
        {
            if (obj is TsDelegate wrapper)
                return Script == wrapper.Script;
            else if (obj is TsScript del)
                return Script == del;

            return false;
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
            if (left is null)
                return right is null;
            return left.Script == right?.Script;
        }

        public static bool operator !=(TsDelegate left, TsDelegate right)
        {
            if (left is null)
                return !(right is null);

            return left.Script != right?.Script;
        }
    }
}