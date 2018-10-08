using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalStream = System.IO.FileStream;

namespace TaffyScript.IO
{
    /// <summary>
    /// TaffyScript wrapper around <see cref="InternalStream"/>.
    /// </summary>
    [TaffyScriptObject]
    public class FileStream : TsStream
    {
        public override string ObjectType => "TaffyScript.IO.FileStream";
        public override Stream Stream => Source;

        /// <summary>
        /// Gets the wrapped <see cref="InternalStream"/>.
        /// </summary>
        public InternalStream Source { get; }

        public FileStream(InternalStream stream)
        {
            Source = stream;
        }

        /// <summary>
        /// Creates a new <see cref="FileStream"/> from a path.
        /// </summary>
        /// <arg name="path" type="string">Absolute or relative path to a file.</arg>
        /// <arg name="mode" type="[FileMode](https://docs.microsoft.com/en-us/dotnet/api/system.io.filemode)">Determines how to open or create the file.</arg>
        /// <arg name="[access]" type="[FileAccess](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileaccess)">Determines how the file can be accessed by the FileStream.</arg>
        /// <arg name="[share]" type="[FileShare](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileshare)">Determines how the file will be shared by processes.</arg>
        /// <arg name="[buffer_size=4096]" type="number">A value greater than 0 indicating the buffer size.</arg>
        /// <arg name="[options]" type="[FileOptions](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileoptions)">Determines additional file options.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.-ctor#System_IO_FileStream__ctor_System_String_System_IO_FileMode_System_IO_FileAccess_System_IO_FileShare_System_Int32_System_IO_FileOptions_</source>
        public FileStream(TsObject[] args)
        {
            switch (args.Length)
            {
                case 2:
                    Source = new InternalStream((string)args[0], (FileMode)(int)args[1]);
                    break;
                case 3:
                    Source = new InternalStream((string)args[0],
                                              (FileMode)(int)args[1],
                                              (FileAccess)(int)args[2]);
                    break;
                case 4:
                    Source = new InternalStream((string)args[0],
                                              (FileMode)(int)args[1],
                                              (FileAccess)(int)args[2],
                                              (FileShare)(int)args[3]);
                    break;
                case 5:
                    Source = new InternalStream((string)args[0],
                                              (FileMode)(int)args[1],
                                              (FileAccess)(int)args[2],
                                              (FileShare)(int)args[3],
                                              (int)args[4]);
                    break;
                case 6:
                    Source = new InternalStream((string)args[0],
                                              (FileMode)(int)args[1],
                                              (FileAccess)(int)args[2],
                                              (FileShare)(int)args[3],
                                              (int)args[4],
                                              (FileOptions)(int)args[5]);
                    break;
                default:
                    throw new ArgumentException($"Not enuough arguments passed to FileStream constructor.");
            }
        }

        public override TsObject GetMember(string name)
        {
            switch (name)
            {
                case "can_read":
                    return Source.CanRead;
                case "can_seek":
                    return Source.CanSeek;
                case "can_timeout":
                    return Source.CanTimeout;
                case "can_write":
                    return Source.CanWrite;
                case "is_async":
                    return Source.IsAsync;
                case "length":
                    return Source.Length;
                case "name":
                    return Source.Name;
                case "position":
                    return Source.Position;
                case "read_timeout":
                    return Source.ReadTimeout;
                case "write_timeout":
                    return Source.WriteTimeout;
                default:
                    if (TryGetDelegate(name, out var del))
                        return del;
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public static implicit operator TsObject(FileStream stream) => new TsInstanceWrapper(stream);
        public static explicit operator FileStream(TsObject obj) => (FileStream)obj.WeakValue;
    }
}
