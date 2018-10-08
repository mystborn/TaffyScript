using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalStream = System.IO.MemoryStream;

namespace TaffyScript.IO
{
    [TaffyScriptObject]
    public class MemoryStream : TsStream
    {
        public override string ObjectType => "TaffyScript.IO.MemoryStream";
        public override Stream Stream => Source;

        public InternalStream Source { get; }

        public MemoryStream(InternalStream stream)
        {
            Source = stream;
        }

        public MemoryStream(TsObject[] args)
        {
            switch(args.Length)
            {
                case 0:
                    Source = new InternalStream();
                    break;
                case 1:
                    switch(args[0].Type)
                    {
                        case VariableType.Real:
                            Source = new InternalStream((int)args[0]);
                            break;
                        default:
                            Source = new InternalStream(((Buffer)args[0]).Memory);
                            break;
                    }
                    break;
                case 3:
                    Source = new InternalStream(((Buffer)args[0]).Memory, (int)args[1], (int)args[2]);
                    break;
            }
        }

        public override TsObject GetMember(string name)
        {
            switch (name)
            {
                case "can_read":
                    return Stream.CanRead;
                case "can_seek":
                    return Stream.CanSeek;
                case "can_timeout":
                    return Stream.CanTimeout;
                case "can_write":
                    return Stream.CanWrite;
                case "capacity":
                    return Source.Capacity;
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

        public override void SetMember(string name, TsObject value)
        {
            switch (name)
            {
                case "capacity":
                    Source.Capacity = (int)value;
                    break;
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

        public static implicit operator TsObject(MemoryStream stream) => new TsInstanceWrapper(stream);
        public static explicit operator MemoryStream(TsObject obj) => (MemoryStream)obj.WeakValue;
    }
}
