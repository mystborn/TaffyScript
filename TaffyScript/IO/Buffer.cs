using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.IO
{
    [TaffyScriptObject]
    public class Buffer : ITsInstance
    {
        private byte[] _memory;

        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public byte this[int position]
        {
            get => Memory[position];
            set => Memory[position] = value;
        }

        public string ObjectType => "TaffyScript.IO.ByteArray";

        public byte[] Memory => _memory;
        public int Position { get; set; }
        public int Length => Memory.Length;

        public Buffer(byte[] source)
        {
            _memory = source;
        }

        public Buffer(int size)
        {
            _memory = new byte[size];
        }

        public Buffer(TsObject[] args)
        {
            _memory = new byte[(int)args[0]];
        }

        public void Write(bool value)
        {
            _memory[Position++] = (byte)(value ? 1 : 0);
        }

        public void Write(byte value)
        {
            _memory[Position++] = value;
        }

        public void Write(sbyte value)
        {
            _memory[Position++] = (byte)value;
        }

        public void Write(ushort value)
        {
            _memory[Position++] = (byte)(value);
            _memory[Position++] = (byte)(value >> 8);
        }

        public void Write(short value)
        {
            _memory[Position++] = (byte)(value);
            _memory[Position++] = (byte)(value >> 8);
        }

        public void Write(uint value)
        {
            _memory[Position++] = (byte)(value);
            _memory[Position++] = (byte)(value >> 8);
            _memory[Position++] = (byte)(value >> 16);
            _memory[Position++] = (byte)(value >> 24);
        }

        public void Write(int value)
        {
            _memory[Position++] = (byte)(value);
            _memory[Position++] = (byte)(value >> 8);
            _memory[Position++] = (byte)(value >> 16);
            _memory[Position++] = (byte)(value >> 24);
        }

        public void Write(ulong value)
        {
            _memory[Position++] = (byte)(value);
            _memory[Position++] = (byte)(value >> 8);
            _memory[Position++] = (byte)(value >> 16);
            _memory[Position++] = (byte)(value >> 24);
            _memory[Position++] = (byte)(value >> 32);
            _memory[Position++] = (byte)(value >> 40);
            _memory[Position++] = (byte)(value >> 48);
            _memory[Position++] = (byte)(value >> 56);
        }

        public void Write(long value)
        {
            _memory[Position++] = (byte)(value);
            _memory[Position++] = (byte)(value >> 8);
            _memory[Position++] = (byte)(value >> 16);
            _memory[Position++] = (byte)(value >> 24);
            _memory[Position++] = (byte)(value >> 32);
            _memory[Position++] = (byte)(value >> 40);
            _memory[Position++] = (byte)(value >> 48);
            _memory[Position++] = (byte)(value >> 56);
        }

        public unsafe void Write(float value)
        {
            // Reinterprets the float as an int while preserving the bit pattern.
            float* fRef = &value;
            int i = *((int*)fRef);
            Write(i);
        }

        public unsafe void Write(double value)
        {
            // Reinterprets the double as a long while preserving the bit pattern.
            double* dRef = &value;
            long l = *((long*)dRef);
            Write(l);
        }

        public void Write(string value)
        {
            foreach (var bit in Encoding.Unicode.GetBytes(value))
                _memory[Position++] = bit;
            _memory[Position++] = 0;
        }

        public bool ReadBool()
        {
            return _memory[Position++] == 1;
        }

        public byte ReadByte()
        {
            return _memory[Position++];
        }

        public sbyte ReadSByte()
        {
            return (sbyte)_memory[Position++];
        }

        public ushort ReadUShort()
        {
            return (ushort)(_memory[Position++] +
                           (_memory[Position++] << 8));
        }

        public short ReadShort()
        {
            return (short)(_memory[Position++] +
                          (_memory[Position++] << 8));
        }

        public uint ReadUInt()
        {
            return (uint)(_memory[Position++] +
                         (_memory[Position++] << 8) +
                         (_memory[Position++] << 16) +
                         (_memory[Position++] << 24));
        }

        public int ReadInt()
        {
            return (_memory[Position++] +
                   (_memory[Position++] << 8) +
                   (_memory[Position++] << 16) +
                   (_memory[Position++] << 24));
        }

        public ulong ReadULong()
        {
            return (ulong)(_memory[Position++] +
                          (_memory[Position++] << 8) +
                          (_memory[Position++] << 16) +
                          (_memory[Position++] << 24) +
                          (_memory[Position++] << 32) +
                          (_memory[Position++] << 40) +
                          (_memory[Position++] << 48) +
                          (_memory[Position++] << 56));
        }

        public long ReadLong()
        {
            return (_memory[Position++] +
                   (_memory[Position++] << 8) +
                   (_memory[Position++] << 16) +
                   (_memory[Position++] << 24) +
                   (_memory[Position++] << 32) +
                   (_memory[Position++] << 40) +
                   (_memory[Position++] << 48) +
                   (_memory[Position++] << 56));
        }

        public unsafe float ReadFloat()
        {
            var i = ReadInt();
            int* iRef = &i;
            return *((float*)iRef);
        }

        public unsafe double ReadDouble()
        {
            var l = ReadLong();
            long* lRef = &l;
            return *((double*)lRef);
        }

        public unsafe string ReadString()
        {
            var start = Position;
            while (_memory[Position++] != 0) ;
            return Encoding.Unicode.GetString(_memory, start, Position - start);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "clear":
                    return clear(args);
                case "clone":
                    return clone(args);
                case "copy_to":
                    return copy_to(args);
                case "get":
                    return get(args);
                case "read_bool":
                    return read_bool(args);
                case "read_byte":
                    return read_byte(args);
                case "read_sbyte":
                    return read_sbyte(args);
                case "read_ushort":
                    return read_ushort(args);
                case "read_short":
                    return read_short(args);
                case "read_uint":
                    return read_uint(args);
                case "read_int":
                    return read_int(args);
                case "read_ulong":
                    return read_ulong(args);
                case "read_long":
                    return read_long(args);
                case "read_float":
                    return read_float(args);
                case "read_double":
                    return read_double(args);
                case "read_string":
                    return read_string(args);
                case "resize":
                    return resize(args);
                case "set":
                    return set(args);
                case "write_bool":
                    return write_bool(args);
                case "write_byte":
                    return write_byte(args);
                case "write_sbyte":
                    return write_sbyte(args);
                case "write_ushort":
                    return write_ushort(args);
                case "write_short":
                    return write_short(args);
                case "write_uint":
                    return write_uint(args);
                case "write_int":
                    return write_int(args);
                case "write_ulong":
                    return write_ulong(args);
                case "write_long":
                    return write_long(args);
                case "write_float":
                    return write_float(args);
                case "write_double":
                    return write_double(args);
                case "write_string":
                    return write_string(args);
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
                case "count":
                case "length":
                    return Length;
                case "position":
                    return Position;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public void SetMember(string name, TsObject value)
        {
            switch(name)
            {
                case "position":
                    Position = (int)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "clear":
                    del = new TsDelegate(clear, scriptName);
                    break;
                case "clone":
                    del = new TsDelegate(clone, scriptName);
                    break;
                case "copy_to":
                    del = new TsDelegate(copy_to, scriptName);
                    break;
                case "get":
                    del = new TsDelegate(get, scriptName);
                    break;
                case "read_bool":
                    del = new TsDelegate(read_bool, scriptName);
                    break;
                case "read_byte":
                    del = new TsDelegate(read_byte, scriptName);
                    break;
                case "read_sbyte":
                    del = new TsDelegate(read_sbyte, scriptName);
                    break;
                case "read_ushort":
                    del = new TsDelegate(read_ushort, scriptName);
                    break;
                case "read_short":
                    del = new TsDelegate(read_short, scriptName);
                    break;
                case "read_uint":
                    del = new TsDelegate(read_uint, scriptName);
                    break;
                case "read_int":
                    del = new TsDelegate(read_int, scriptName);
                    break;
                case "read_ulong":
                    del = new TsDelegate(read_ulong, scriptName);
                    break;
                case "read_long":
                    del = new TsDelegate(read_long, scriptName);
                    break;
                case "read_float":
                    del = new TsDelegate(read_float, scriptName);
                    break;
                case "read_double":
                    del = new TsDelegate(read_double, scriptName);
                    break;
                case "read_string":
                    del = new TsDelegate(read_string, scriptName);
                    break;
                case "resize":
                    del = new TsDelegate(resize, scriptName);
                    break;
                case "set":
                    del = new TsDelegate(set, scriptName);
                    break;
                case "write_bool":
                    del = new TsDelegate(write_bool, scriptName);
                    break;
                case "write_byte":
                    del = new TsDelegate(write_byte, scriptName);
                    break;
                case "write_sbyte":
                    del = new TsDelegate(write_sbyte, scriptName);
                    break;
                case "write_ushort":
                    del = new TsDelegate(write_ushort, scriptName);
                    break;
                case "write_short":
                    del = new TsDelegate(write_short, scriptName);
                    break;
                case "write_uint":
                    del = new TsDelegate(write_uint, scriptName);
                    break;
                case "write_int":
                    del = new TsDelegate(write_int, scriptName);
                    break;
                case "write_ulong":
                    del = new TsDelegate(write_ulong, scriptName);
                    break;
                case "write_long":
                    del = new TsDelegate(write_long, scriptName);
                    break;
                case "write_float":
                    del = new TsDelegate(write_float, scriptName);
                    break;
                case "write_double":
                    del = new TsDelegate(write_double, scriptName);
                    break;
                case "write_string":
                    del = new TsDelegate(write_string, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        public TsObject clear(TsObject[] args)
        {
            Array.Clear(Memory, 0, Length);
            return TsObject.Empty;
        }

        public TsObject clone(TsObject[] args)
        {
            var copy = new Buffer(Length);
            Array.Copy(Memory, copy.Memory, Length);
            copy.Position = Position;
            return copy;
        }

        public TsObject copy_to(TsObject[] args)
        {
            var dest = (Buffer)args[0];
            var length = (int)args[1];
            switch (args.Length)
            {
                case 2:
                    Array.Copy(Memory, dest.Memory, length);
                    break;
                case 4:
                    Array.Copy(Memory, (int)args[2], dest.Memory, (int)args[3], length);
                    break;
                default:
                    throw new ArgumentException($"Not enough arguments passed to {ObjectType}.{nameof(copy_to)}");
            }
            return TsObject.Empty;
        }

        public TsObject get(TsObject[] args)
        {
            return Memory[(int)args[0]];
        }

        public TsObject read_bool(TsObject[] args) => ReadBool();
        public TsObject read_byte(TsObject[] args) => ReadByte();
        public TsObject read_sbyte(TsObject[] args) => ReadSByte();
        public TsObject read_ushort(TsObject[] args) => ReadUShort();
        public TsObject read_short(TsObject[] args) => ReadShort();
        public TsObject read_uint(TsObject[] args) => ReadUInt();
        public TsObject read_int(TsObject[] args) => ReadInt();
        public TsObject read_ulong(TsObject[] args) => ReadULong();
        public TsObject read_long(TsObject[] args) => ReadLong();
        public TsObject read_float(TsObject[] args) => ReadFloat();
        public TsObject read_double(TsObject[] args) => ReadDouble();
        public TsObject read_string(TsObject[] args) => ReadString();

        public TsObject resize(TsObject[] args)
        {
            Array.Resize(ref _memory, (int)args[0]);
            return TsObject.Empty;
        }

        public TsObject set(TsObject[] args)
        {
            Memory[(int)args[0]] = (byte)args[1];
            return TsObject.Empty;
        }

        public TsObject write_bool(TsObject[] args)
        {
            Write((bool)args[0]);
            return Position;
        }

        public TsObject write_byte(TsObject[] args)
        {
            Write((byte)args[0]);
            return Position;
        }

        public TsObject write_sbyte(TsObject[] args)
        {
            Write((sbyte)args[0]);
            return Position;
        }

        public TsObject write_ushort(TsObject[] args)
        {
            Write((ushort)args[0]);
            return Position;
        }

        public TsObject write_short(TsObject[] args)
        {
            Write((short)args[0]);
            return Position;
        }

        public TsObject write_uint(TsObject[] args)
        {
            Write((uint)args[0]);
            return Position;
        }

        public TsObject write_int(TsObject[] args)
        {
            Write((int)args[0]);
            return Position;
        }

        public TsObject write_ulong(TsObject[] args)
        {
            Write((ulong)args[0]);
            return Position;
        }

        public TsObject write_long(TsObject[] args)
        {
            Write((long)args[0]);
            return Position;
        }

        public TsObject write_float(TsObject[] args)
        {
            Write((float)args[0]);
            return Position;
        }

        public TsObject write_double(TsObject[] args)
        {
            Write((double)args[0]);
            return Position;
        }

        public TsObject write_string(TsObject[] args)
        {
            Write((string)args[0]);
            return Position;
        }

        public static implicit operator TsObject(Buffer array) => new TsInstanceWrapper(array);
        public static explicit operator Buffer(TsObject obj) => (Buffer)obj.WeakValue;
    }
}
