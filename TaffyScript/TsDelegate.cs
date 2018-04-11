using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public delegate TsObject TsScript(TsInstance target, TsObject[] args);

    public enum TsScriptScope
    {
        Instance,
        NeedsTarget,
        Global
    }

    public class TsDelegate : IEquatable<TsDelegate>
    {
        public TsScriptScope ScriptScope { get; }
        public TsScript Script { get; private set; }
        public TsInstance Target { get; private set; }
        public bool Disposed { get; private set; } = false;
        public string Name { get; }

        public TsDelegate(TsScript script, string name)
        {
            Target = null;
            Script = script;
            ScriptScope = TsScriptScope.Global;
            Name = name;
        }

        public TsDelegate(TsScript script, string name, TsInstance target)
        {
            ScriptScope = TsScriptScope.Instance;
            Target = target;
            Script = script;
            target.Destroyed += OnTargetDestroyed;
            Name = name;
        }

        /// <summary>
        /// Creates a copy of a TsDelegate with a new target.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target"></param>
        public TsDelegate(TsDelegate original, TsInstance target)
        {
            ScriptScope = TsScriptScope.Instance;
            Script = original.Script;
            Name = original.Name;
            Target = target;
        }

        public TsObject Invoke(params TsObject[] args)
        {
            // If the script needs a target, get it from the first index of the args array.
            // This will make it easier to invoke Delegates from TS.

            // If the target has been destroyed, this delegate will still exist.
            // The program should throw to avoid UB.
            // Potential Alts:
            //   - Continue to use the destroyed target. Without dereferencing it here
            //     it will still be a valid object. However this leads to it's own UB.
            //     Once an object is destroyed, it should be assumed it no longer wants to be used.
            //   - See if an instance with the same id exists. Much worse UB.

            if (ScriptScope == TsScriptScope.NeedsTarget)
            {
                var target = args[0].GetInstance();
                var scriptArgs = new TsObject[args.Length - 1];
                Array.Copy(args, 1, scriptArgs, 0, args.Length - 1);
                return Script(target, scriptArgs);
            }
            else if (Disposed)
                throw new ObjectDisposedException("Target", "The target of this script has been destroyed.");

            return Script(Target, args);
        }

        public TsObject Invoke(TsInstance target, params TsObject[] args)
        {
            Console.WriteLine(Name);
            if (target is null && ScriptScope != TsScriptScope.Global)
                throw new ArgumentNullException("target", "This script requires a target to invoke.");
            return Script(target, args);
        }

        private void OnTargetDestroyed(TsInstance inst)
        {
            Disposed = true;
            Target = null;
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
            if (left != null)
                return left.Equals(right);
            else return right is null;
        }

        public static bool operator !=(TsDelegate left, TsDelegate right)
        {
            if (left != null)
                return !left.Equals(right);
            else return !(right is null);
        }
    }
}