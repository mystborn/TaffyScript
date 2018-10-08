using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalReader = System.IO.TextReader;

namespace TaffyScript.IO
{
    public abstract class TextReader : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public abstract string ObjectType { get; }
        public abstract InternalReader Reader { get; }

        public virtual TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "close":
                    return close(args);
                case "dispose":
                    return dispose(args);
                case "peek":
                    return peek(args);
                case "read":
                    return read(args);
                case "read_line":
                    return read_line(args);
                case "read_to_end":
                    return read_to_end(args);
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
            if (TryGetDelegate(name, out var del))
                return del;
            throw new MissingMemberException(ObjectType, name);
        }

        public virtual void SetMember(string name, TsObject value)
        {
            throw new MissingMemberException(ObjectType, name);
        }

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "close":
                    del = new TsDelegate(close, scriptName);
                    break;
                case "dispose":
                    del = new TsDelegate(dispose, scriptName);
                    break;
                case "peek":
                    del = new TsDelegate(peek, scriptName);
                    break;
                case "read":
                    del = new TsDelegate(read, scriptName);
                    break;
                case "read_line":
                    del = new TsDelegate(read_line, scriptName);
                    break;
                case "read_to_end":
                    del = new TsDelegate(read_to_end, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        public TsObject close(TsObject[] args)
        {
            Reader.Close();
            return TsObject.Empty;
        }

        public TsObject dispose(TsObject[] args)
        {
            Reader.Dispose();
            return TsObject.Empty;
        }

        public TsObject peek(TsObject[] args)
        {
            return Reader.Peek();
        }

        public TsObject read(TsObject[] args)
        {
            return Reader.Read();
        }

        public TsObject read_line(TsObject[] args)
        {
            return Reader.ReadLine();
        }

        public TsObject read_to_end(TsObject[] args)
        {
            return Reader.ReadToEnd();
        }

        public static implicit operator TsObject(TextReader reader) => new TsInstanceWrapper(reader);
        public static explicit operator TextReader(TsObject obj) => (TextReader)obj.WeakValue;
    }
}
