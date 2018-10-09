using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.IO
{
    /// <summary>
    /// Base class for TaffyScript streams.
    /// </summary>
    /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream</source>
    /// <property name="can_read" type="bool" access="get">
    ///     <summary>Determines if the stream can be read from.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.canread</source>
    /// </property>
    /// <property name="can_seek" type="bool" access="get">
    ///     <summary>Determines if the stream supports seeking.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.canseek</source>
    /// </property>
    /// <property name="can_timeout" type="bool" access="get">
    ///     <summary>Determines if the stream can time out.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.cantimeout</source>
    /// </property>
    /// <property name="can_write" type="bool" access="get">
    ///     <summary>Determines if the stream can be written to.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.canwrite</source>
    /// </property>
    /// <property name="length" type="number" access="get">
    ///     <summary>Gets the length in bytes of the stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.length</source>
    /// </property>
    /// <property name="position" type="number" access="both">
    ///     <summary>Gets or sets the position within the stream.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.position</source>
    /// </property>
    /// <property name="read_timeout" type="number" access="both">
    ///     <summary>Gets or sets the length of time in milliseconds the stream will attempt to read before timing out.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.readtimeout</source>
    /// </property>
    /// <property name="write_timeout" type="number" access="both">
    ///     <summary>Gets or set the length of tume in milliseconds the stream will attempt to write before timing out.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.writetimeout</source>
    /// </property>
    [TaffyScriptObject("TaffyScript.IO.Stream")]
    public abstract class TsStream : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public abstract string ObjectType { get; }
        public abstract Stream Stream { get; }

        public virtual TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "copy_to":
                    return copy_to(args);
                case "dispose":
                    return dispose(args);
                case "flush":
                    return flush(args);
                case "read_byte":
                    return read_byte(args);
                case "seek":
                    return seek(args);
                case "set_length":
                    return set_length(args);
                case "write_byte":
                    return write_byte(args);
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
                case "can_read":
                    return Stream.CanRead;
                case "can_seek":
                    return Stream.CanSeek;
                case "can_timeout":
                    return Stream.CanTimeout;
                case "can_write":
                    return Stream.CanWrite;
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

        public virtual void SetMember(string name, TsObject value)
        {
            switch(name)
            {
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

        public virtual bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch(scriptName)
            {
                case "copy_to":
                    del = new TsDelegate(copy_to, scriptName);
                    break;
                case "dispose":
                    del = new TsDelegate(dispose, scriptName);
                    break;
                case "flush":
                    del = new TsDelegate(flush, scriptName);
                    break;
                case "read_byte":
                    del = new TsDelegate(read_byte, scriptName);
                    break;
                case "seek":
                    del = new TsDelegate(seek, scriptName);
                    break;
                case "set_length":
                    del = new TsDelegate(set_length, scriptName);
                    break;
                case "write_byte":
                    del = new TsDelegate(write_byte, scriptName);
                    break;
                default:
                    throw new MissingMethodException(ObjectType, scriptName);
            }
            return true;
        }

        /// <summary>
        /// Reads the bytes from this stream and writes them to another.
        /// </summary>
        /// <arg name="other" type="[Stream]({{site.baseurl}}/docs/TaffyScript/IO/Stream)">The stream to write to.</arg>
        /// <arg name="[buffer_size]" type="number">The size of the buffer.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.copyto</source>
        /// <returns>null</returns>
        public TsObject copy_to(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    Stream.CopyTo(((TsStream)args[0]).Stream);
                    break;
                case 2:
                    Stream.CopyTo(((TsStream)args[0]).Stream, (int)args[1]);
                    break;
                default:
                    throw new ArgumentException($"Not enough arguments provided to {nameof(copy_to)}");
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Releases all resources used by this stream.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.dispose</source>
        /// <returns>null</returns>
        public TsObject dispose(TsObject[] args)
        {
            Stream.Dispose();
            return TsObject.Empty;
        }

        /// <summary>
        /// Clears all buffers for this stream and writes any buffered data to the underlying device.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.flush</source>
        /// <returns>null</returns>
        public TsObject flush(TsObject[] args)
        {
            Stream.Flush();
            return TsObject.Empty;
        }

        /// <summary>
        /// Reads a sequence of bytes from the stream into a buffer and returns the number of bytes read.
        /// </summary>
        /// <arg name="buffer" type="[Buffer]({{site.baseurl}}/docs/TaffyScript.IO.Buffer)">The buffer to write the data to.</arg>
        /// <arg name="offset" type="number">The position in the buffer to write the data.</arg>
        /// <arg name="count" type="number">The maximum number of bytes to be read.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.read</source>
        /// <returns>number</returns>
        public TsObject read(TsObject[] args)
        {
            var buffer = (Buffer)args[0];
            var count = Stream.Read(buffer.Memory, (int)args[1], (int)args[2]);
            return count;
        }

        /// <summary>
        /// Reads the next byte from the stream.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.readbyte</source>
        /// <returns>number</returns>
        public TsObject read_byte(TsObject[] args)
        {
            return Stream.ReadByte();
        }

        /// <summary>
        /// Sets the position within the stream.
        /// </summary>
        /// <arg name="offset" type="number">A byte offset relative to the origin parameter.</arg>
        /// <arg name="origin" type="[SeekOrigin](https://docs.microsoft.com/en-us/dotnet/api/system.io.seekorigin)">Indicates the starting point to obtain the new position.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.seek</source>
        /// <returns>number</returns>
        public TsObject seek(TsObject[] args)
        {
            return Stream.Seek((long)args[0], (SeekOrigin)(int)args[1]);
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <arg name="length" type="number">The desired length of the stream in bytes.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.setlength</source>
        /// <returns>null</returns>
        public TsObject set_length(TsObject[] args)
        {
            Stream.SetLength((long)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a sequence of bytes from a buffer to the stream.
        /// </summary>
        /// <arg name="buffer" type="[Buffer]({{site.baseurl}}/docs/TaffyScript.IO.Buffer)">The buffer to read the data from.</arg>
        /// <arg name="offset" type="number">The position in the buffer to start reading from.</arg>
        /// <arg name="count" type="number">The number of bytes to read from the buffer.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.write</source>
        /// <returns>null</returns>
        public TsObject write(TsObject[] args)
        {
            var buffer = (Buffer)args[0];
            Stream.Write(buffer.Memory, (int)args[1], (int)args[2]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Writes a byte to the stream.
        /// </summary>
        /// <arg name="byte" type="number">The byte to write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.writebyte</source>
        /// <returns>null</returns>
        public TsObject write_byte(TsObject[] args)
        {
            Stream.WriteByte((byte)args[0]);
            return TsObject.Empty;
        }

        public static implicit operator TsObject(TsStream stream)
        {
            return new TsInstanceWrapper(stream);
        }

        public static explicit operator TsStream(TsObject obj) => (TsStream)obj.WeakValue;
    }
}
