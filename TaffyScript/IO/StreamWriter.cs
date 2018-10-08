using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalWriter = System.IO.StreamWriter;

namespace TaffyScript.IO
{
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

        public TsObject GetMember(string name)
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

        public void SetMember(string name, TsObject value)
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
