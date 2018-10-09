using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalStream = System.IO.MemoryStream;

namespace TaffyScript.IO
{
    /// <summary>
    /// Creates a stream whose backing store is memory.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.memorystream?view=netframework-4.7</source>
    /// <property name="can_read" type="bool" access="get">
    ///     <summary>Determines if the stream can be read from.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.memorystream.canread?view=netframework-4.7</source>
    /// </property>
    /// <property name="can_seek" type="bool" access="get">
    ///     <summary>Determines if the stream supports seeking.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.memorystream.canseek?view=netframework-4.7</source>
    /// </property>
    /// <property name="can_write" type="bool" access="get">
    ///     <summary>Determines if the stream can be written to.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.memorystream.canwrite?view=netframework-4.7</source>
    /// </property>
    /// <property name="capacity" type="number" access="both">
    ///     <summary>Gets or sets the number of bytes allocated for this stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.memorystream.capacity?view=netframework-4.7</source>
    /// </property>
    /// <property name="length" type="number" access="get">
    ///     <summary>Gets the length in bytes of the stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.memorystream.length?view=netframework-4.7</source>
    /// </property>
    /// <property name="position" type="number" access="both">
    ///     <summary>Gets or sets the position within the stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.memorystream.position?view=netframework-4.7</source>
    /// </property>
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
