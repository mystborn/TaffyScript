using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalReader = System.IO.StreamReader;

namespace TaffyScript.IO
{
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
