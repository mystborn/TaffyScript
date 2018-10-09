using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalReader = System.IO.StreamReader;

namespace TaffyScript.IO
{
    /// <summary>
    /// Reads characters from a byte stream using a particular encoding.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=netframework-4.7</source>
    /// <property name="base_stream" type="[Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream)" access="get">
    ///     <summary>Gets the underlying stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader.basestream?view=netframework-4.7</source>
    /// </property>
    /// <property name="current_encoding" type="string" access="get">
    ///     <summary>Gets the name of the encoding the StreamReader is using.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader.currentencoding?view=netframework-4.7</source>
    /// </property>
    /// <property name="end_of_stream" type="bool" access="get">
    ///     <summary>Determines if the stream position is at the end of the stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader.endofstream?view=netframework-4.7</source>
    /// </property>
    [TaffyScriptObject]
    public class StreamReader : TextReader
    {
        private TsStream _baseStream = null;

        public override string ObjectType => "System.IO.StreamReader";
        public override System.IO.TextReader Reader => Source;

        public InternalReader Source { get; }

        public TsStream BaseStream
        {
            get
            {
                if(_baseStream is null)
                {
                    if (Source.BaseStream is System.IO.FileStream fs)
                        _baseStream = new FileStream(fs);
                    else
                        _baseStream = new WrappedStream(Source.BaseStream);
                }
                return _baseStream;
            }
        }

        public StreamReader(InternalReader reader)
        {
            Source = reader;
        }

        /// <summary>
        /// Initializes a new StreamReader from a Stream or a path to a file.
        /// </summary>
        /// <arg name="source" type="string or [Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream)">A path to a file or a stream to write to.</arg>
        /// <arg name="[encoding=*utf-8*]" type="string">The name of the character encoding to use.</arg>
        /// <arg name="[detect_encoding_from_byte_order_marks]" type="bool">Determines whether to look for byte order marks at the beginning of the file.</arg>
        /// <arg name="[buffer_size]" type="number">The minimum size of the buffer in bytes.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter.-ctor?view=netframework-4.7</source>
        public StreamReader(TsObject[] args)
        {
            if(args[0].Type == VariableType.String)
            {
                var path = (string)args[0];
                switch(args.Length)
                {
                    case 1:
                        Source = new InternalReader(path);
                        break;
                    case 2:
                        Source = new InternalReader(path, Encoding.GetEncoding((string)args[1]));
                        break;
                    case 3:
                        Source = new InternalReader(path, Encoding.GetEncoding((string)args[1]), (bool)args[2]);
                        break;
                    case 4:
                        Source = new InternalReader(path, Encoding.GetEncoding((string)args[1]), (bool)args[2], (int)args[3]);
                        break;
                    default:
                        throw new ArgumentException($"Invalid number of arguments passed to constructor of {ObjectType}");
                }
            }
            else
            {
                _baseStream = (TsStream)args[0];
                switch(args.Length)
                {
                    case 1:
                        Source = new InternalReader(_baseStream.Stream);
                        break;
                    case 2:
                        Source = new InternalReader(_baseStream.Stream, Encoding.GetEncoding((string)args[1]));
                        break;
                    case 3:
                        Source = new InternalReader(_baseStream.Stream, Encoding.GetEncoding((string)args[1]), (bool)args[2]);
                        break;
                    case 4:
                        Source = new InternalReader(_baseStream.Stream, Encoding.GetEncoding((string)args[1]), (bool)args[2], (int)args[3]);
                        break;
                    default:
                        throw new ArgumentException($"Invalid number of arguments passed to constructor of {ObjectType}");
                }
            }
        }

        public override TsObject GetMember(string name)
        {
            switch(name)
            {
                case "base_stream":
                    return BaseStream;
                case "current_encoding":
                    return Source.CurrentEncoding.EncodingName;
                case "end_of_stream":
                    return Source.EndOfStream;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public static implicit operator TsObject(StreamReader reader) => new TsInstanceWrapper(reader);
        public static explicit operator StreamReader(TsObject obj) => (StreamReader)obj.WeakValue;
    }
}
