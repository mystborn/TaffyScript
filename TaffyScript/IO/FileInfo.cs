using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalSource = System.IO.FileInfo;

namespace TaffyScript.IO
{
    /// <summary>
    /// Provides mechanisms for the creation, copying, deletion, moving, and opening of files.
    /// </summary>
    /// <property name="attributes" type="[FileAttributes](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileattributes)" access="both">
    ///     <summary>Gets or sets the attributes for the directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.attributes</source>
    /// </property>
    /// <property name="creation_time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)" access="both">
    ///     <summary>Gets or sets the creation time of the directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.creationtime</source>
    /// </property>
    /// <property name="creation_time_utc" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)" access="both">
    ///     <summary>Gets or sets the creation time in coordinated universal time of the directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.creationtimeutc</source>
    /// </property>
    /// <property name="exists" type="bool" access="get">
    ///     <summary>Determines if the directory exists.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.exists</source>
    /// </property>
    /// <property name="extension" type="string" access="get">
    ///     <summary>Gets the extension part of the directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.extension</source>
    /// </property>
    /// <property name="full_name" type="string" access="get">
    ///     <summary>Gets the full path of the directory</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.fullname</source>
    /// </property>
    /// <property name="last_access_time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)" access="both">
    ///     <summary>Gets or sets the last time the directory was accessed.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.lastaccesstime</source>
    /// </property>
    /// <property name="last_access_time_utc" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)" access="both">
    ///     <summary>Gets or sets the last time the directory was accessed in coordinated universal time.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.lastaccesstimeutc</source>
    /// </property>
    /// <property name="last_write_time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)" access="both">
    ///     <summary>Gets or sets the last time the directory was written to.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.lastwritetime</source>
    /// </property>
    /// <property name="last_write_time_utc" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)" access="both">
    ///     <summary>Gets or sets the last time the directory was written to in coordinated universal time.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.lastwritetimeutc</source>
    /// </property>
    /// <property name="name" type="string" access="get">
    ///     <summary>Gets the name of the directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.name</source>
    /// </property>
    /// <property name="directory" type="[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)" access="get">
    ///     <summary>Gets an instance of the parent directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.directory</source>
    /// </property>
    /// <property name="directory_name" type="string" access="get">
    ///     <summary>Gets the name of the parent directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.directoryname</source>
    /// </property>
    /// <property name="is_read_only" type="bool" access="both">
    ///     <summary>Gets or sets a value that determines if the file read only.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.isreadonly</source>
    /// </property>
    /// <property name="length" type="number" access="get">
    ///     <summary>Gets the size, in bytes, of the current file.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.length</source>
    /// </property>
    [TaffyScriptObject]
    public class FileInfo : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.IO.FileInfo";
        public InternalSource Source { get; }

        public FileInfo(InternalSource source)
        {
            Source = source;
        }

        public FileInfo(TsObject[] args)
        {
            Source = new InternalSource((string)args[0]);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "append_text":
                    return append_text(args);
                case "copy_to":
                    return copy_to(args);
                case "create":
                    return create(args);
                case "create_text":
                    return create_text(args);
                case "decrypt":
                    return decrypt(args);
                case "delete":
                    return delete(args);
                case "encrypt":
                    return encrypt(args);
                case "move_to":
                    return move_to(args);
                case "open":
                    return open(args);
                case "open_read":
                    return open_read(args);
                case "open_text":
                    return open_text(args);
                case "open_write":
                    return open_write(args);
                case "refresh":
                    return refresh(args);
                case "replace":
                    return replace(args);
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

        public TsObject GetMember(string name)
        {
            switch (name)
            {
                case "attributes":
                    return (int)Source.Attributes;
                case "creation_time":
                    return new TsDateTime(Source.CreationTime);
                case "creation_time_utc":
                    return new TsDateTime(Source.CreationTimeUtc);
                case "directory":
                    return new DirectoryInfo(Source.Directory);
                case "directory_name":
                    return Source.DirectoryName;
                case "exists":
                    return Source.Exists;
                case "extension":
                    return Source.Extension;
                case "full_name":
                    return Source.FullName;
                case "is_read_only":
                    return Source.IsReadOnly;
                case "last_access_time":
                    return new TsDateTime(Source.LastAccessTime);
                case "last_access_time_utc":
                    return new TsDateTime(Source.LastAccessTimeUtc);
                case "last_write_time":
                    return new TsDateTime(Source.LastWriteTime);
                case "last_write_time_utc":
                    return new TsDateTime(Source.LastWriteTimeUtc);
                case "length":
                    return Source.Length;
                case "name":
                    return Source.Name;
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
                case "attributes":
                    Source.Attributes = (FileAttributes)(int)value;
                    break;
                case "creation_time":
                    Source.CreationTime = ((TsDateTime)value).Source;
                    break;
                case "creation_time_utc":
                    Source.CreationTimeUtc = ((TsDateTime)value).Source;
                    break;
                case "is_read_only":
                    Source.IsReadOnly = (bool)value;
                    break;
                case "last_access_time":
                    Source.LastAccessTime = ((TsDateTime)value).Source;
                    break;
                case "last_access_time_utc":
                    Source.LastAccessTimeUtc = ((TsDateTime)value).Source;
                    break;
                case "last_write_time":
                    Source.LastWriteTime = ((TsDateTime)value).Source;
                    break;
                case "last_write_time_utc":
                    Source.LastWriteTimeUtc = ((TsDateTime)value).Source;
                    break;
                default:
                    throw new MissingMemberException(ObjectType, name);
            }
        }

        public bool TryGetDelegate(string scriptName, out TsDelegate del)
        {
            switch (scriptName)
            {
                case "append_text":
                    del = new TsDelegate(append_text, scriptName);
                    break;
                case "copy_to":
                    del = new TsDelegate(copy_to, scriptName);
                    break;
                case "create":
                    del = new TsDelegate(create, scriptName);
                    break;
                case "create_text":
                    del = new TsDelegate(create_text, scriptName);
                    break;
                case "decrypt":
                    del = new TsDelegate(decrypt, scriptName);
                    break;
                case "delete":
                    del = new TsDelegate(delete, scriptName);
                    break;
                case "encrypt":
                    del = new TsDelegate(encrypt, scriptName);
                    break;
                case "move_to":
                    del = new TsDelegate(move_to, scriptName);
                    break;
                case "open":
                    del = new TsDelegate(open, scriptName);
                    break;
                case "open_read":
                    del = new TsDelegate(open_read, scriptName);
                    break;
                case "open_text":
                    del = new TsDelegate(open_text, scriptName);
                    break;
                case "open_write":
                    del = new TsDelegate(open_write, scriptName);
                    break;
                case "refresh":
                    del = new TsDelegate(refresh, scriptName);
                    break;
                case "replace":
                    del = new TsDelegate(replace, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a StreamWriter that appends text to this file.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.appendtext?view=netframework-4.7</source>
        /// <returns>[StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter)</returns>
        public TsObject append_text(TsObject[] args)
        {
            return new StreamWriter(Source.AppendText());
        }

        /// <summary>
        /// Copies the file to a new file, optionally allowing the overwrite of an existing file.
        /// </summary>
        /// <arg name="new_file_name" type="string">The name of the new file to copy to.</arg>
        /// <arg name="[overwrite=false]" type="bool">Determines whether this script can overwrite an existing file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.copyto?view=netframework-4.7</source>
        /// <returns>[FileInfo]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo)</returns>
        public TsObject copy_to(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new FileInfo(Source.CopyTo((string)args[0]));
                case 2:
                    return new FileInfo(Source.CopyTo((string)args[0], (bool)args[1]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(copy_to)}");
            }
        }

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.create?view=netframework-4.7</source>
        /// <returns>[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)</returns>
        public TsObject create(TsObject[] args)
        {
            return new FileStream(Source.Create());
        }

        /// <summary>
        /// Creates a StreamWriter that writes to a new text file.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.createtext?view=netframework-4.7</source>
        /// <returns>[StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter)</returns>
        public TsObject create_text(TsObject[] args)
        {
            return new StreamWriter(Source.CreateText());
        }

        /// <summary>
        /// Decrypts a file that was encrypted by the current account using encrypt.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.decrypt?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject decrypt(TsObject[] args)
        {
            Source.Decrypt();
            return TsObject.Empty;
        }

        /// <summary>
        /// Permanently deletes this file.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.delete?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject delete(TsObject[] args)
        {
            Source.Delete();
            return TsObject.Empty;
        }

        /// <summary>
        /// Encrypts a file so only the account used to encrypt the file can decrypt it.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.encrypt?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject encrypt(TsObject[] args)
        {
            Source.Encrypt();
            return TsObject.Empty;
        }

        /// <summary>
        /// Moves the file to a new location.
        /// </summary>
        /// <arg name="new_file_name" type="string">The name of the new file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.moveto?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject move_to(TsObject[] args)
        {
            Source.MoveTo((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Opens the file with the specified options.
        /// </summary>
        /// <arg name="mode" type="[FileMode](https://docs.microsoft.com/en-us/dotnet/api/system.io.filemode)">Specifies the mode in which to open the file.</arg>
        /// <arg name="[access]" type="[FileAccess](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileaccess)">Determines the access with which to open the file.</arg>
        /// <arg name="[share]" type="[FileShare](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileshare)">Determines the type of access other FileStreams have to this file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.open?view=netframework-4.7</source>
        /// <returns>[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)</returns>
        public TsObject open(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new FileStream(Source.Open((FileMode)(int)args[0]));
                case 2:
                    return new FileStream(Source.Open((FileMode)(int)args[0], (FileAccess)(int)args[1]));
                case 3:
                    return new FileStream(Source.Open((FileMode)(int)args[0], (FileAccess)(int)args[1], (FileShare)(int)args[2]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(open)}");
            }
        }

        /// <summary>
        /// Creates a read-only FileStream.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.openread?view=netframework-4.7</source>
        /// <returns>[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)</returns>
        public TsObject open_read(TsObject[] args)
        {
            return new FileStream(Source.OpenRead());
        }

        /// <summary>
        /// Creates a StreamReader that reads from an existing text file.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.opentext?view=netframework-4.7</source>
        /// <returns>[StreamReader]({{site.baseurl}}/docs/TaffyScript/IO/StreamReader)</returns>
        public TsObject open_text(TsObject[] args)
        {
            return new StreamReader(Source.OpenText());
        }

        /// <summary>
        /// Creates a write-only FileStream.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.openwrite?view=netframework-4.7</source>
        /// <returns>[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)</returns>
        public TsObject open_write(TsObject[] args)
        {
            return new FileStream(Source.OpenWrite());
        }

        /// <summary>
        /// Refreshes the state of this file.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.refresh</source>
        /// <returns>null</returns>
        public TsObject refresh(TsObject[] args)
        {
            Source.Refresh();
            return TsObject.Empty;
        }

        /// <summary>
        /// Replaces the contents of the specified file with the contents of this file.
        /// </summary>
        /// <arg name="destination_file_name" type="string">The name of a file to replace.</arg>
        /// <arg name="destination_backup_file_name" type="string">The name of a file with which to create a backup of the replaced file. Will not be created if this argument is null.</arg>
        /// <arg name="[ignore_metadata_errors]" type="bool">Determines whether to ignore merge errors from the replaced file to the replacement file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo.replace?view=netframework-4.7</source>
        /// <returns>[FileInfo]({{site.baseurl}}/docs/TaffyScript/IO/FileInfo)</returns>
        public TsObject replace(TsObject[] args)
        {
            switch (args.Length)
            {
                case 2:
                    return new FileInfo(Source.Replace((string)args[0], (string)args[1]));
                case 3:
                    return new FileInfo(Source.Replace((string)args[0], args[1].GetStringOrNull(), (bool)args[2]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(replace)}");
            }
        }

        public static implicit operator TsObject(FileInfo fileInfo) => new TsInstanceWrapper(fileInfo);
        public static explicit operator FileInfo(TsObject obj) => (FileInfo)obj.WeakValue;
    }
}
