using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalWriter = System.IO.StreamWriter;

namespace TaffyScript.IO
{
    /// <summary>
    /// Implements a TextWriter for writing characters to a stream in a particular encoding.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter?view=netframework-4.7</source>
    /// <property name="auto_flush" type="bool" access="both">
    ///     <summary>Determines if the StreamWriter will flush its buffer to the underlying stream after every call to write.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter.autoflush?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public class StreamWriter : TextWriter
    {
        private TsStream _baseStream = null;

        public override string ObjectType => "TaffyScript.IO.StreamWriter";
        public override System.IO.TextWriter Writer => Source;

        public InternalWriter Source { get; }

        public TsStream BaseStream
        {
            get
            {
                if (_baseStream is null)
                {
                    if (Source.BaseStream is System.IO.FileStream fs)
                        _baseStream = new FileStream(fs);
                    else
                        _baseStream = new WrappedStream(Source.BaseStream);
                }
                return _baseStream;
            }
        }

        public StreamWriter(InternalWriter writer)
        {
            Source = writer;
        }

        /// <summary>
        /// Initializes a new StreamWriter from a Stream or a path to a file.
        /// </summary>
        /// <arg name="source" type="string or [Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream)">A path to a file or a stream to write to.</arg>
        /// <arg name="[append]" type="bool">Determines whether to append data to a file or overwrite it. Ignored if initialized from a stream.</arg>
        /// <arg name="[encoding=*utf-8*]" type="string">The name of the character encoding to use.</arg>
        /// <arg name="[buffer_size]" type="number">The buffer size, in bytes.</arg>
        /// <arg name="[leave_open]" type="bool">Determines whether to leave the underlying stream open after the StreamWriter is disposed. Ignored it initialized from a path.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter.-ctor?view=netframework-4.7</source>
        public StreamWriter(TsObject[] args)
        {
            if (args[0].Type == VariableType.String)
            {
                var path = (string)args[0];
                switch (args.Length)
                {
                    case 1:
                        Source = new InternalWriter(path);
                        break;
                    case 2:
                        Source = new InternalWriter(path, (bool)args[1]);
                        break;
                    case 3:
                        Source = new InternalWriter(path, (bool)args[1], Encoding.GetEncoding((string)args[2]));
                        break;
                    case 4:
                        Source = new InternalWriter(path, (bool)args[1], Encoding.GetEncoding((string)args[2]), (int)args[3]);
                        break;
                    default:
                        throw new ArgumentException($"Invalid number of arguments passed to the constructor of {ObjectType}");
                }
            }
            else
            {
                _baseStream = (TsStream)args[0];
                switch (args.Length)
                {
                    case 1:
                        Source = new InternalWriter(_baseStream.Stream);
                        break;
                    case 3:
                        Source = new InternalWriter(_baseStream.Stream, Encoding.GetEncoding((string)args[2]));
                        break;
                    case 4:
                        Source = new InternalWriter(_baseStream.Stream, Encoding.GetEncoding((string)args[2]), (int)args[3]);
                        break;
                    case 5:
                        Source = new InternalWriter(_baseStream.Stream, Encoding.GetEncoding((string)args[2]), (int)args[3], (bool)args[4]);
                        break;
                    default:
                        throw new ArgumentException($"Invalid number of arguments passed to the constructor of {ObjectType}");
                }
            }
        }

        public override TsObject GetMember(string name)
        {
            switch (name)
            {
                case "auto_flush":
                    return Source.AutoFlush;
                case "base_stream":
                    return BaseStream;
                case "encoding":
                    return Source.Encoding.EncodingName;
                case "new_line":
                    return Source.NewLine;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public override void SetMember(string name, TsObject value)
        {
            switch (name)
            {
                case "auto_flush":
                    Source.AutoFlush = (bool)value;
                    break;
                case "new_line":
                    Source.NewLine = (string)value;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public static implicit operator TsObject(StreamWriter writer) => new TsInstanceWrapper(writer);
        public static explicit operator StreamWriter(TsObject obj) => (StreamWriter)obj.WeakValue;
    }
}
