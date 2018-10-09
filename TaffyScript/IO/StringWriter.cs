using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalWriter = System.IO.StringWriter;

namespace TaffyScript.IO
{
    /// <summary>
    /// Implements a TextWriter for writing information to a string. The information is stored in a [StringBuilder]({{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder).
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stringwriter?view=netframework-4.7</source>
    [TaffyScriptObject]
    public class StringWriter : TextWriter
    {
        public override string ObjectType => "TaffyScript.IO.StringWriter";
        public override System.IO.TextWriter Writer => Source;

        public InternalWriter Source { get; }

        public StringWriter(InternalWriter writer)
        {
            Source = writer;
        }

        /// <summary>
        /// Initializes a new StringWriter.
        /// </summary>
        /// <arg name="[string_builder]" type="[StringBuilder]({{site.baseurl}}/docs/TaffyScript/Strings/StringBuilder)">The StringBuilder to write to. Defaults to creating a new StringBuilder.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stringwriter.-ctor?view=netframework-4.7</source>
        public StringWriter(TsObject[] args)
        {
            if (args is null)
                Source = new InternalWriter();

            switch(args.Length)
            {
                case 0:
                    Source = new InternalWriter();
                    break;
                case 1:
                    Source = new InternalWriter(((TaffyScript.Strings.StringBuilder)args[0]).Source);
                    break;
            }
        }

        public override TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "dispose":
                    return dispose(args);
                case "flush":
                    return flush(args);
                case "to_string":
                    return to_string(args);
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

        public override bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "dispose":
                    del = new TsDelegate(dispose, scriptName);
                    break;
                case "flush":
                    del = new TsDelegate(flush, scriptName);
                    break;
                case "to_string":
                    del = new TsDelegate(to_string, scriptName);
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
        /// Returns a string containing the characters written to the current StringWriter so far.
        /// </summary>
        /// <returns>string</returns>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stringwriter.tostring?view=netframework-4.7</source>
        public TsObject to_string(TsObject[] args)
        {
            return Source.ToString();
        }
    }
}
