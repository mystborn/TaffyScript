using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Collections
{
    /// <summary>
    /// Supports iteration over a collection.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1?view=netframework-4.7</source>
    [TaffyScriptObject("TaffyScript.Collections.Enumerator")]
    public abstract class TsEnumerator : IEnumerator<TsObject>, ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public virtual TsObject Current => current;
        object IEnumerator.Current => Current;

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1.current</source>
        /// <type>object</type>
        public abstract TsObject current { get; }

        public abstract string ObjectType { get; }

        public static TsEnumerator Wrap(IEnumerator<TsObject> enumerator)
        {
            return new WrappedEnumerator(enumerator);
        }

        public TsEnumerator(TsObject[] args)
        {
        }

        public virtual void Dispose()
        {
            dispose(null);
        }

        public virtual bool MoveNext()
        {
            return (bool)move_next(null);
        }

        public virtual void Reset()
        {
            reset(null);
        }

        public virtual TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "dispose":
                    Dispose();
                    return TsObject.Empty;
                case "move_next":
                    return MoveNext();
                case "reset":
                    Reset();
                    return TsObject.Empty;
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
        }

        public TsDelegate GetDelegate(string scriptName)
        {
            if (TryGetDelegate(scriptName, out var del))
                return del;
            throw new MissingMethodException(ObjectType, scriptName);
        }

        public virtual TsObject GetMember(string name)
        {
            switch(name)
            {
                case "current":
                    return Current;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public virtual void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "dispose":
                    del = new TsDelegate(dispose, scriptName);
                    break;
                case "move_next":
                    del = new TsDelegate(move_next, scriptName);
                    break;
                case "reset":
                    del = new TsDelegate(reset, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Disposes any dynamic resources held by the enumerator.
        /// </summary>
        /// <returns>null</returns>
        public abstract TsObject dispose(TsObject[] args);

        /// <summary>
        /// Advances the enumerator to the next element in the collection.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.movenext</source>
        /// <returns>bool</returns>
        public abstract TsObject move_next(TsObject[] args);

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the last element in the collection.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.reset</source>
        /// <returns>null</returns>
        public abstract TsObject reset(TsObject[] args);

        public static implicit operator TsObject(TsEnumerator enumerator) => new TsInstanceWrapper(enumerator);
        public static explicit operator TsEnumerator(TsObject obj) => (TsEnumerator)obj.WeakValue;
    }

    internal class WrappedEnumerator : TsEnumerator
    {
        private IEnumerator<TsObject> _source;

        public override string ObjectType => "TaffyScript.Collections.WrappedEnumerator";

        public override TsObject Current => _source.Current;
        public override TsObject current => _source.Current;

        public WrappedEnumerator(IEnumerator<TsObject> enumerator)
            : base(null)
        {
            _source = enumerator;
        }

        public override void Dispose()
        {
            _source.Dispose();
        }

        public override TsObject dispose(TsObject[] args)
        {
            _source.Dispose();
            return TsObject.Empty;
        }

        public override bool Equals(object obj)
        {
            return _source.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _source.GetHashCode();
        }

        public override bool MoveNext()
        {
            return _source.MoveNext();
        }

        public override TsObject move_next(TsObject[] args)
        {
            return MoveNext();
        }

        public override void Reset()
        {
            _source.Reset();
        }

        public override TsObject reset(TsObject[] args)
        {
            Reset();
            return TsObject.Empty;
        }

        public override string ToString()
        {
            return _source.ToString();
        }
    }
}
