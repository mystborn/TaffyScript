using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.IO
{
    /// <summary>
    /// Base class for TaffyScript streams.
    /// </summary>
    public abstract class TsStream : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public abstract string ObjectType { get; }
        public abstract Stream Stream { get; }

        public virtual TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "close":
                    return close(args);
                case "copy_to":
                    return copy_to(args);
                case "dispose":
                    return dispose(args);
                case "flush":
                    return flush(args);
                case "read_byte":
                    return read_byte(args);
                case "seek":
                    return seek(args);
                case "set_length":
                    return set_length(args);
                case "write_byte":
                    return write_byte(args);
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
                case "can_read":
                    return Stream.CanRead;
                case "can_seek":
                    return Stream.CanSeek;
                case "can_timeout":
                    return Stream.CanTimeout;
                case "can_write":
                    return Stream.CanWrite;
                case "length":
                    return Stream.Length;
                case "position":
                    return Stream.Position;
                case "read_timeout":
                    return Stream.ReadTimeout;
                case "write_timeout":
                    return Stream.WriteTimeout;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public virtual void SetMember(string name, TsObject value)
        {
            switch(name)
            {
                case "position":
                    Stream.Position = (long)value;
                    break;
                case "read_timeout":
                    Stream.ReadTimeout = (int)value;
                    break;
                case "write_timeout":
                    Stream.WriteTimeout = (int)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "close":
                    del = new TsDelegate(close, scriptName);
                    break;
                case "copy_to":
                    del = new TsDelegate(copy_to, scriptName);
                    break;
                case "dispose":
                    del = new TsDelegate(dispose, scriptName);
                    break;
                case "flush":
                    del = new TsDelegate(flush, scriptName);
                    break;
                case "read_byte":
                    del = new TsDelegate(read_byte, scriptName);
                    break;
                case "seek":
                    del = new TsDelegate(seek, scriptName);
                    break;
                case "set_length":
                    del = new TsDelegate(set_length, scriptName);
                    break;
                case "write_byte":
                    del = new TsDelegate(write_byte, scriptName);
                    break;
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
            return true;
        }

        public TsObject close(TsObject[] args)
        {
            Stream.Close();
            return TsObject.Empty;
        }

        public TsObject copy_to(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    Stream.CopyTo(((TsStream)args[0]).Stream);
                    break;
                case 2:
                    Stream.CopyTo(((TsStream)args[0]).Stream, (int)args[1]);
                    break;
                default:
                    throw new ArgumentException($"Not enough arguments provided to {nameof(copy_to)}");
            }
            return TsObject.Empty;
        }

        public TsObject dispose(TsObject[] args)
        {
            Stream.Dispose();
            return TsObject.Empty;
        }

        public TsObject flush(TsObject[] args)
        {
            Stream.Flush();
            return TsObject.Empty;
        }

        public TsObject read_byte(TsObject[] args)
        {
            return Stream.ReadByte();
        }

        public TsObject seek(TsObject[] args)
        {
            return Stream.Seek((long)args[0], (SeekOrigin)(int)args[1]);
        }

        public TsObject set_length(TsObject[] args)
        {
            Stream.SetLength((long)args[0]);
            return TsObject.Empty;
        }

        public TsObject write_byte(TsObject[] args)
        {
            Stream.WriteByte((byte)args[0]);
            return TsObject.Empty;
        }

        public static implicit operator TsObject(TsStream stream)
        {
            return new TsInstanceWrapper(stream);
        }

        public static explicit operator TsStream(TsObject obj) => (TsStream)obj.WeakValue;
    }
}
