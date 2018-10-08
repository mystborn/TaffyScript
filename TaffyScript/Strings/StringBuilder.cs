using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalBuilder = System.Text.StringBuilder;

namespace TaffyScript.Strings
{
    [TaffyScriptObject]
    public class StringBuilder : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.Strings.StringBuilder";

        public InternalBuilder Source { get; }

        public StringBuilder(InternalBuilder source)
        {
            Source = source;
        }

        public StringBuilder(TsObject[] args)
        {
            switch(args.Length)
            {
                case 0:
                    Source = new InternalBuilder();
                    break;
                case 1:
                    Source = new InternalBuilder((int)args[0]);
                    break;
                case 2:
                    Source = new InternalBuilder((int)args[0], (int)args[1]);
                    break;
            }
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "append":
                    return append(args);
                case "append_bool":
                    return append_bool(args);
                case "append_byte":
                    return append_byte(args);
                case "append_sbyte":
                    return append_sbyte(args);
                case "append_ushort":
                    return append_ushort(args);
                case "append_short":
                    return append_short(args);
                case "append_uint":
                    return append_uint(args);
                case "append_int":
                    return append_int(args);
                case "append_ulong":
                    return append_ulong(args);
                case "append_long":
                    return append_long(args);
                case "append_float":
                    return append_float(args);
                case "append_double":
                    return append_double(args);
                case "append_line":
                    return append_line(args);
                case "clear":
                    return clear(args);
                case "get":
                    return get(args);
                case "insert":
                    return insert(args);
                case "insert_bool":
                    return insert_bool(args);
                case "insert_byte":
                    return insert_byte(args);
                case "insert_sbyte":
                    return insert_sbyte(args);
                case "insert_ushort":
                    return insert_ushort(args);
                case "insert_short":
                    return insert_short(args);
                case "insert_uint":
                    return insert_uint(args);
                case "insert_int":
                    return insert_int(args);
                case "insert_ulong":
                    return insert_ulong(args);
                case "insert_long":
                    return insert_long(args);
                case "insert_float":
                    return insert_float(args);
                case "insert_double":
                    return insert_double(args);
                case "remove":
                    return remove(args);
                case "replace":
                    return replace(args);
                case "set":
                    return set(args);
                case "to_string":
                    return to_string(args);
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
                case "capacity":
                    return Source.Capacity;
                case "length":
                case "count":
                    return Source.Length;
                case "max_capacity":
                    return Source.MaxCapacity;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            switch (name)
            {
                case "capacity":
                    Source.Capacity = (int)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "append":
                    del = new TsDelegate(append, scriptName);
                    break;
                case "append_bool":
                    del = new TsDelegate(append_bool, scriptName);
                    break;
                case "append_byte":
                    del = new TsDelegate(append_byte, scriptName);
                    break;
                case "append_sbyte":
                    del = new TsDelegate(append_sbyte, scriptName);
                    break;
                case "append_ushort":
                    del = new TsDelegate(append_ushort, scriptName);
                    break;
                case "append_short":
                    del = new TsDelegate(append_short, scriptName);
                    break;
                case "append_uint":
                    del = new TsDelegate(append_uint, scriptName);
                    break;
                case "append_int":
                    del = new TsDelegate(append_int, scriptName);
                    break;
                case "append_ulong":
                    del = new TsDelegate(append_ulong, scriptName);
                    break;
                case "append_long":
                    del = new TsDelegate(append_long, scriptName);
                    break;
                case "append_float":
                    del = new TsDelegate(append_float, scriptName);
                    break;
                case "append_double":
                    del = new TsDelegate(append_double, scriptName);
                    break;
                case "append_line":
                    del = new TsDelegate(append_line, scriptName);
                    break;
                case "clear":
                    del = new TsDelegate(clear, scriptName);
                    break;
                case "get":
                    del = new TsDelegate(get, scriptName);
                    break;
                case "insert":
                    del = new TsDelegate(insert, scriptName);
                    break;
                case "insert_bool":
                    del = new TsDelegate(insert_bool, scriptName);
                    break;
                case "insert_byte":
                    del = new TsDelegate(insert_byte, scriptName);
                    break;
                case "insert_sbyte":
                    del = new TsDelegate(insert_sbyte, scriptName);
                    break;
                case "insert_ushort":
                    del = new TsDelegate(insert_ushort, scriptName);
                    break;
                case "insert_short":
                    del = new TsDelegate(insert_short, scriptName);
                    break;
                case "insert_uint":
                    del = new TsDelegate(insert_uint, scriptName);
                    break;
                case "insert_int":
                    del = new TsDelegate(insert_int, scriptName);
                    break;
                case "insert_ulong":
                    del = new TsDelegate(insert_ulong, scriptName);
                    break;
                case "insert_long":
                    del = new TsDelegate(insert_long, scriptName);
                    break;
                case "insert_float":
                    del = new TsDelegate(insert_float, scriptName);
                    break;
                case "insert_double":
                    del = new TsDelegate(insert_double, scriptName);
                    break;
                case "remove":
                    del = new TsDelegate(remove, scriptName);
                    break;
                case "replace":
                    del = new TsDelegate(replace, scriptName);
                    break;
                case "set":
                    del = new TsDelegate(set, scriptName);
                    break;
                case "to_string":
                    del = new TsDelegate(to_string, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        public TsObject append(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    Source.Append((string)args[0]);
                    break;
                case 3:
                    Source.Append((string)args[0], (int)args[1], (int)args[2]);
                    break;
                default:
                    throw new ArgumentException($"Not enough arguments passed to {ObjectType}.{nameof(append)}");
            }
            Source.Append((string)args[0]);
            return this;
        }

        public TsObject append_bool(TsObject[] args)
        {
            Source.Append((bool)args[0]);
            return this;
        }

        public TsObject append_sbyte(TsObject[] args)
        {
            Source.Append((sbyte)args[0]);
            return this;
        }

        public TsObject append_byte(TsObject[] args)
        {
            Source.Append((byte)args[0]);
            return this;
        }

        public TsObject append_ushort(TsObject[] args)
        {
            Source.Append((ushort)args[0]);
            return this;
        }

        public TsObject append_short(TsObject[] args)
        {
            Source.Append((short)args[0]);
            return this;
        }

        public TsObject append_uint(TsObject[] args)
        {
            Source.Append((uint)args[0]);
            return this;
        }

        public TsObject append_int(TsObject[] args)
        {
            Source.Append((int)args[0]);
            return this;
        }

        public TsObject append_ulong(TsObject[] args)
        {
            Source.Append((ulong)args[0]);
            return this;
        }

        public TsObject append_long(TsObject[] args)
        {
            Source.Append((long)args[0]);
            return this;
        }

        public TsObject append_float(TsObject[] args)
        {
            Source.Append((float)args[0]);
            return this;
        }

        public TsObject append_double(TsObject[] args)
        {
            Source.Append((double)args[0]);
            return this;
        }

        public TsObject append_line(TsObject[] args)
        {
            if (args is null)
                Source.AppendLine();
            else
            {
                switch(args.Length)
                {
                    case 0:
                        Source.AppendLine();
                        break;
                    case 1:
                        Source.AppendLine((string)args[0]);
                        break;
                }
            }
            return this;
        }

        public TsObject clear(TsObject[] args)
        {
            Source.Clear();
            return this;
        }

        public TsObject get(TsObject[] args)
        {
            return Source[(int)args[0]];
        }

        public TsObject insert(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    Source.Insert((int)args[0], (string)args[1]);
                    break;
                case 3:
                    Source.Insert((int)args[0], (string)args[1], (int)args[2]);
                    break;
            }
            return this;
        }

        public TsObject insert_bool(TsObject[] args)
        {
            Source.Insert((int)args[0], (bool)args[1]);
            return this;
        }

        public TsObject insert_byte(TsObject[] args)
        {
            Source.Insert((int)args[0], (byte)args[1]);
            return this;
        }

        public TsObject insert_sbyte(TsObject[] args)
        {
            Source.Insert((int)args[0], (sbyte)args[1]);
            return this;
        }

        public TsObject insert_ushort(TsObject[] args)
        {
            Source.Insert((int)args[0], (ushort)args[1]);
            return this;
        }

        public TsObject insert_short(TsObject[] args)
        {
            Source.Insert((int)args[0], (short)args[1]);
            return this;
        }

        public TsObject insert_uint(TsObject[] args)
        {
            Source.Insert((int)args[0], (uint)args[1]);
            return this;
        }

        public TsObject insert_int(TsObject[] args)
        {
            Source.Insert((int)args[0], (int)args[1]);
            return this;
        }

        public TsObject insert_ulong(TsObject[] args)
        {
            Source.Insert((int)args[0], (ulong)args[1]);
            return this;
        }

        public TsObject insert_long(TsObject[] args)
        {
            Source.Insert((int)args[0], (long)args[1]);
            return this;
        }

        public TsObject insert_float(TsObject[] args)
        {
            Source.Insert((int)args[0], (float)args[1]);
            return this;
        }

        public TsObject insert_double(TsObject[] args)
        {
            Source.Insert((int)args[0], (double)args[1]);
            return this;
        }

        public TsObject remove(TsObject[] args)
        {
            Source.Remove((int)args[0], (int)args[1]);
            return this;
        }

        public TsObject replace(TsObject[] args)
        {
            Source.Replace((string)args[0], (string)args[1]);
            return this;
        }

        public TsObject set(TsObject[] args)
        {
            Source[(int)args[0]] = (char)args[1];
            return this;
        }

        public TsObject to_string(TsObject[] args)
        {
            if (args is null)
                return Source.ToString();

            switch(args.Length)
            {
                case 0:
                    return Source.ToString();
                case 2:
                    return Source.ToString((int)args[0], (int)args[1]);
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(to_string)}");
            }
        }

        public static implicit operator TsObject(StringBuilder sb) => new TsInstanceWrapper(sb);
        public static explicit operator StringBuilder(TsObject obj) => (StringBuilder)obj.WeakValue;
    }
}
