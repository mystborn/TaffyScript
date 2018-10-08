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
    /// <property name="current" type="object" access="get">
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1.current</source>
    ///     <summary>Gets the element in the collection at the current position of the enumerator.</summary>
    /// </property>
    public struct TsEnumerator : IEnumerator<TsObject>, ITsInstance
    {
        private IEnumerator<TsObject> _source;

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public TsObject Current => _source.Current;
        object IEnumerator.Current => _source.Current;

        public string ObjectType => "TaffyScript.Collections.TsEnumerator";

        public TsEnumerator(IEnumerable<TsObject> source)
        {
            _source = source.GetEnumerator();
        }

        public TsEnumerator(IEnumerator<TsObject> source)
        {
            _source = source;
        }

        public void Dispose()
        {
            _source.Dispose();
        }

        public bool MoveNext()
        {
            return _source.MoveNext();
        }

        public void Reset()
        {
            _source.Reset();
        }

        public IEnumerable<TsObject> Iterate()
        {
            while (MoveNext())
                yield return Current;
        }

        public TsObject Call(string scriptName, params TsObject[] args)
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

        public TsObject GetMember(string name)
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

        public void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
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
        public TsObject dispose(TsObject[] args)
        {
            Dispose();
            return TsObject.Empty;
        }

        /// <summary>
        /// Advances the enumerator to the next element in the collection.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.movenext</source>
        /// <returns>bool</returns>
        public TsObject move_next(TsObject[] args)
        {
            return MoveNext();
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the last element in the collection.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.reset</source>
        /// <returns>null</returns>
        public TsObject reset(TsObject[] args)
        {
            Reset();
            return TsObject.Empty;
        }

        public static implicit operator TsObject(TsEnumerator enumerator) => new TsInstanceWrapper(enumerator);
        public static explicit operator TsEnumerator(TsObject obj) => (TsEnumerator)obj.WeakValue;
    }
}
