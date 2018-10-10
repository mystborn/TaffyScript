using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalReader = System.IO.TextReader;

namespace TaffyScript.IO
{
    /// <summary>
    /// Represents a reader that can read a sequential series of characters.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader?view=netframework-4.7</source>
    [TaffyScriptObject]
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

        /// <summary>
        /// Releases all resource used by this TextReader.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader.dispose?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject dispose(TsObject[] args)
        {
            Reader.Dispose();
            return TsObject.Empty;
        }

        /// <summary>
        /// Gets the next character as a number without actually reading it.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader.peek?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject peek(TsObject[] args)
        {
            return Reader.Peek();
        }

        /// <summary>
        /// Reads the next character as a number.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader.read?view=netframework-4.7</source>
        /// <returns>number</returns>
        public TsObject read(TsObject[] args)
        {
            return Reader.Read();
        }

        /// <summary>
        /// Reads a line of characters and returns the data as a string.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader.readline?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject read_line(TsObject[] args)
        {
            return Reader.ReadLine();
        }

        /// <summary>
        /// Reads all characters from the current position to the end of the TextReader and returns them as one string.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader.readtoend?view=netframework-4.7</source>
        /// <returns>string</returns>
        public TsObject read_to_end(TsObject[] args)
        {
            return Reader.ReadToEnd();
        }

        public static implicit operator TsObject(TextReader reader) => new TsInstanceWrapper(reader);
        public static explicit operator TextReader(TsObject obj) => (TextReader)obj.WeakValue;
    }
}
