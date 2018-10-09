using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalWriter = System.IO.TextWriter;

namespace TaffyScript.IO
{
    /// <summary>
    /// Represents a writer that can write a sequential series of characters.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter?view=netframework-4.7</source>
    /// <property name="culture" type="string" access="get">
    ///     <summary>Gets the name of the culture that controls the formatting.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.formatprovider?view=netframework-4.7</source>
    /// </property>
    /// <property name="encoding" type="string" access="get">
    ///     <summary>Gets the name of the character encoding in which the output is written.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.encoding?view=netframework-4.7</source>
    /// </property>
    /// <property name="new_line" type="string" access="both">
    ///     <summary>Gets or sets the line terminator string.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.newline?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public abstract class TextWriter : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public abstract string ObjectType { get; }
        public abstract InternalWriter Writer { get; }

        public virtual TsObject Call(string scriptName, params TsObject[] args)
        {
            switch(scriptName)
            {
                case "dispose":
                    return dispose(args);
                case "flush":
                    return flush(args);
                case "write":
                    return write(args);
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
                case "write_long":
                    return write_long(args);
                case "write_ulong":
                    return write_ulong(args);
                case "write_float":
                    return write_float(args);
                case "write_double":
                    return write_double(args);
                case "write_line":
                    return write_line(args);
                case "write_line_bool":
                    return write_line_bool(args);
                case "write_line_byte":
                    return write_line_byte(args);
                case "write_line_sbyte":
                    return write_line_sbyte(args);
                case "write_line_ushort":
                    return write_line_ushort(args);
                case "write_line_short":
                    return write_line_short(args);
                case "write_line_uint":
                    return write_line_uint(args);
                case "write_line_int":
                    return write_line_int(args);
                case "write_line_long":
                    return write_line_long(args);
                case "write_line_ulong":
                    return write_line_ulong(args);
                case "write_line_float":
                    return write_line_float(args);
                case "write_line_double":
                    return write_line_double(args);
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
                case "encoding":
                    return Writer.Encoding.EncodingName;
                case "culture":
                    if (Writer.FormatProvider is CultureInfo ci)
                        return ci.Name;
                    else
                        return TsObject.Empty;
                case "new_line":
                    return Writer.NewLine;
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
                case "new_line":
                    Writer.NewLine = (string)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "dispose":
                    del = new TsDelegate(dispose, scriptName);
                    break;
                case "flush":
                    del = new TsDelegate(flush, scriptName);
                    break;
                case "write":
                    del = new TsDelegate(write, scriptName);
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
                case "write_long":
                    del = new TsDelegate(write_long, scriptName);
                    break;
                case "write_ulong":
                    del = new TsDelegate(write_ulong, scriptName);
                    break;
                case "write_float":
                    del = new TsDelegate(write_float, scriptName);
                    break;
                case "write_double":
                    del = new TsDelegate(write_double, scriptName);
                    break;
                case "write_line":
                    del = new TsDelegate(write_line, scriptName);
                    break;
                case "write_line_bool":
                    del = new TsDelegate(write_line_bool, scriptName);
                    break;
                case "write_line_byte":
                    del = new TsDelegate(write_line_byte, scriptName);
                    break;
                case "write_line_sbyte":
                    del = new TsDelegate(write_line_sbyte, scriptName);
                    break;
                case "write_line_ushort":
                    del = new TsDelegate(write_line_ushort, scriptName);
                    break;
                case "write_line_short":
                    del = new TsDelegate(write_line_short, scriptName);
                    break;
                case "write_line_uint":
                    del = new TsDelegate(write_line_uint, scriptName);
                    break;
                case "write_line_int":
                    del = new TsDelegate(write_line_int, scriptName);
                    break;
                case "write_line_long":
                    del = new TsDelegate(write_line_long, scriptName);
                    break;
                case "write_line_ulong":
                    del = new TsDelegate(write_line_ulong, scriptName);
                    break;
                case "write_line_float":
                    del = new TsDelegate(write_line_float, scriptName);
                    break;
                case "write_line_double":
                    del = new TsDelegate(write_line_double, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Releases all resources used by the TextWriter.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textreader.dispose?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject dispose(TsObject[] args)
        {
            Writer.Dispose();
            return TsObject.Empty;
        }

        /// <summary>
        /// Clears all buffers and writes any buffered data to be written to the underlying device.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.flush?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject flush(TsObject[] args)
        {
            Writer.Flush();
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given string.
        /// </summary>
        /// <arg name="value" type="string">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write(TsObject[] args)
        {
            Writer.Write((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given bool.
        /// </summary>
        /// <arg name="value" type="bool">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_bool(TsObject[] args)
        {
            Writer.Write((bool)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given byte.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_byte(TsObject[] args)
        {
            Writer.Write((byte)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given sbyte.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_sbyte(TsObject[] args)
        {
            Writer.Write((sbyte)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given ushort.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_ushort(TsObject[] args)
        {
            Writer.Write((ushort)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given short.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_short(TsObject[] args)
        {
            Writer.Write((short)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given uint.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_uint(TsObject[] args)
        {
            Writer.Write((uint)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given int.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_int(TsObject[] args)
        {
            Writer.Write((int)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given ulong.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_ulong(TsObject[] args)
        {
            Writer.Write((ulong)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given long.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_long(TsObject[] args)
        {
            Writer.Write((long)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given float.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_float(TsObject[] args)
        {
            Writer.Write((float)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given double.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.write?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_double(TsObject[] args)
        {
            Writer.Write((double)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a line terminator.
        /// </summary>
        /// <arg name="[value]" type="string">An optional string to write before the line terminator.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line(TsObject[] args)
        {
            if (args is null)
                Writer.WriteLine();
            else
            {
                switch(args.Length)
                {
                    case 0:
                        Writer.WriteLine();
                        break;
                    case 1:
                        Writer.Write((string)args[0]);
                        break;
                }
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given bool.
        /// </summary>
        /// <arg name="value" type="bool">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_bool(TsObject[] args)
        {
            Writer.WriteLine((bool)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given byte.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_byte(TsObject[] args)
        {
            Writer.WriteLine((byte)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given sbyte.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_sbyte(TsObject[] args)
        {
            Writer.WriteLine((sbyte)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given ushort.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_ushort(TsObject[] args)
        {
            Writer.WriteLine((ushort)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given short.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_short(TsObject[] args)
        {
            Writer.WriteLine((short)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given uint.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_uint(TsObject[] args)
        {
            Writer.WriteLine((uint)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given int.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_int(TsObject[] args)
        {
            Writer.WriteLine((int)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given ulong.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_ulong(TsObject[] args)
        {
            Writer.WriteLine((ulong)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given long.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_long(TsObject[] args)
        {
            Writer.WriteLine((long)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given float.
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_float(TsObject[] args)
        {
            Writer.WriteLine((float)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes the text representation of the given double
        /// </summary>
        /// <arg name="value" type="number">The value to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.textwriter.writeline?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject write_line_double(TsObject[] args)
        {
            Writer.WriteLine((double)args[0]);
            return TsObject.Empty;
        }

        public static implicit operator TsObject(TextWriter writer) => new TsInstanceWrapper(writer);
        public static explicit operator TextWriter(TsObject obj) => (TextWriter)obj.WeakValue;
    }
}
