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
    /// Provides a Stream for a file.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream?view=netframework-4.7</source>
    /// <property name="can_read" type="bool" access="get">
    ///     <summary>Determines if the stream can be read from.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.canread?view=netframework-4.7</source>
    /// </property>
    /// <property name="can_seek" type="bool" access="get">
    ///     <summary>Determines if the stream supports seeking.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.canseek?view=netframework-4.7</source>
    /// </property>
    /// <property name="can_write" type="bool" access="get">
    ///     <summary>Determines if the stream can be written to.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.canwrite?view=netframework-4.7</source>
    /// </property>
    /// <property name="is_async" type="bool" access="get">
    ///     <summary>Determines if the file stream was opened asynchronously or synchronously.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.isasync?view=netframework-4.7</source>
    /// </property>
    /// <property name="length" type="number" access="get">
    ///     <summary>Gets the length in bytes of the stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.length?view=netframework-4.7</source>
    /// </property>
    /// <property name="name" type="string" access="get">
    ///     <summary>Gets the absolute path of the file opened in the FileStream</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.name?view=netframework-4.7</source>
    /// </property>
    /// <property name="position" type="number" access="both">
    ///     <summary>Gets or sets the position within the stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.position?view=netframework-4.7</source>
    /// </property>
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
        /// Creates a new FileStream from a path.
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
