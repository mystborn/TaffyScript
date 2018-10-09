using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;
using InternalSource = System.IO.DirectoryInfo;

namespace TaffyScript.IO
{
    /// <summary>
    /// Exposes scripts for creating, moving, and iterating through directories.
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
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.exists</source>
    /// </property>
    /// <property name="extension" type="string" access="get">
    ///     <summary>Gets the extension part of the directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.extension</source>
    /// </property>
    /// <property name="full_name" type="string" access="get">
    ///     <summary>Gets the full path of the directory</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.fullname</source>
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
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.name</source>
    /// </property>
    /// <property name="parent" type="[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)" access="get">
    ///     <summary>Gets the parent directory.</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.parent</source>
    /// </property>
    /// <property name="root" type="[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)" access="get">
    ///     <summary>Gets the root portion of the directory</summary>
    ///     <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.root</source>
    /// </property>
    [TaffyScriptObject]
    public sealed class DirectoryInfo : ITsInstance
    {
        public TsObject this[string memberName]
        {
            get => GetMember(memberName);
            set => SetMember(memberName, value);
        }

        public string ObjectType => "TaffyScript.IO.DirectoryInfo";
        public InternalSource Source { get; }

        public DirectoryInfo(InternalSource source)
        {
            Source = source;
        }

        public DirectoryInfo(TsObject[] args)
        {
            Source = new InternalSource((string)args[0]);
        }

        public TsObject Call(string scriptName, params TsObject[] args)
        {
            switch (scriptName)
            {
                case "create":
                    return create(args);
                case "create_subdirectory":
                    return create_subdirectory(args);
                case "delete":
                    return delete(args);
                case "get_directories":
                    return get_directories(args);
                case "get_files":
                    return get_files(args);
                case "move_to":
                    return move_to(args);
                case "refresh":
                    return refresh(args);
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
                case "exists":
                    return Source.Exists;
                case "extension":
                    return Source.Extension;
                case "full_name":
                    return Source.FullName;
                case "last_access_time":
                    return new TsDateTime(Source.LastAccessTime);
                case "last_access_time_utc":
                    return new TsDateTime(Source.LastAccessTimeUtc);
                case "last_write_time":
                    return new TsDateTime(Source.LastWriteTime);
                case "last_write_time_utc":
                    return new TsDateTime(Source.LastWriteTimeUtc);
                case "name":
                    return Source.Name;
                case "parent":
                    return new DirectoryInfo(Source.Parent);
                case "root":
                    return new DirectoryInfo(Source.Root);
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
                case "create":
                    del = new TsDelegate(create, scriptName);
                    break;
                case "create_subdirectory":
                    del = new TsDelegate(create_subdirectory, scriptName);
                    break;
                case "delete":
                    del = new TsDelegate(delete, scriptName);
                    break;
                case "get_directories":
                    del = new TsDelegate(get_directories, scriptName);
                    break;
                case "get_files":
                    del = new TsDelegate(get_files, scriptName);
                    break;
                case "move_to":
                    del = new TsDelegate(move_to, scriptName);
                    break;
                case "refresh":
                    del = new TsDelegate(refresh, scriptName);
                    break;
                default:
                    del = null;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.create?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject create(TsObject[] args)
        {
            Source.Create();
            return TsObject.Empty;
        }

        /// <summary>
        /// Creates a subdirectory with the specified path. The path can be relative to this directory.
        /// </summary>
        /// <arg name="path" type="string">The specified path.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.createsubdirectory?view=netframework-4.7</source>
        /// <returns>[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)</returns>
        public TsObject create_subdirectory(TsObject[] args)
        {
            return new DirectoryInfo(Source.CreateSubdirectory((string)args[0]));
        }

        /// <summary>
        /// Deletes this directory.
        /// </summary>
        /// <arg name="[delete_subdirectories=false]" type="bool">Determines whether to delete files and subdirectories. If this is false and the directory is not empty, this throws an exception.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.delete?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject delete(TsObject[] args)
        {
            if (args is null)
                Source.Delete();

            switch (args.Length)
            {
                case 0:
                    Source.Delete();
                    break;
                case 1:
                    Source.Delete((bool)args[0]);
                    break;
            }

            return TsObject.Empty;
        }

        /// <summary>
        /// Returns a collection of directory information in the current directory.
        /// </summary>
        /// <arg name="[search_pattern]" type="string">The search string to compare against the names of directories. Can contain literal path characters and the wildcards * and ?</arg>
        /// <arg name="[search_option=SearchOption.TopDirectoryOnly]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption)">Specifies whether the search operation should only include the current directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.enumeratedirectories?view=netframework-4.7</source>
        /// <returns>Enumerable</returns>
        public TsObject enumerate_directories(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return new WrappedEnumerable(Source.EnumerateDirectories().Select(di => (TsObject)new DirectoryInfo(di)));
                case 1:
                    return new WrappedEnumerable(Source.EnumerateDirectories((string)args[0]).Select(di => (TsObject)new DirectoryInfo(di)));
                case 2:
                    return new WrappedEnumerable(Source.EnumerateDirectories((string)args[0], (SearchOption)(int)args[1]).Select(di => (TsObject)new DirectoryInfo(di)));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(enumerate_directories)}");
            }
        }

        /// <summary>
        /// Returns a collection of file information in the current directory.
        /// </summary>
        /// <arg name="[search_pattern]" type="string">The search string to compare against the names of files. Can contain literal path characters and the wildcards * and ?</arg>
        /// <arg name="[search_option=SearchOption.TopDirectoryOnly]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption)">Specifies whether the search operation should only include the current directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.enumeratefiles?view=netframework-4.7</source>
        /// <returns>Enumerable</returns>
        public TsObject enumerate_file(TsObject[] args)
        {
            switch(args?.Length)
            {
                case null:
                case 0:
                    return new WrappedEnumerable(Source.EnumerateFiles().Select(di => (TsObject)new FileInfo(di)));
                case 1:
                    return new WrappedEnumerable(Source.EnumerateFiles((string)args[0]).Select(di => (TsObject)new FileInfo(di)));
                case 2:
                    return new WrappedEnumerable(Source.EnumerateFiles((string)args[0], (SearchOption)(int)args[1]).Select(di => (TsObject)new FileInfo(di)));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(enumerate_file)}");
            }
        }

        /// <summary>
        /// Returns an array of the subdirectories in the directory.
        /// </summary>
        /// <arg name="[search_pattern]" type="string">The search string to compare against the names of directories. Can contain literal path characters and the wildcards * and ?</arg>
        /// <arg name="[search_option=SearchOption.TopDirectoryOnly]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption)">Specifies whether the search operation should only include the current directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.getdirectories?view=netframework-4.7</source>
        /// <returns>array</returns>
        public TsObject get_directories(TsObject[] args)
        {
            InternalSource[] directories;
            switch (args?.Length)
            {
                case null:
                case 0:
                    directories = Source.GetDirectories();
                    break;
                case 1:
                    directories = Source.GetDirectories((string)args[0]);
                    break;
                case 2:
                    directories = Source.GetDirectories((string)args[0], (SearchOption)(int)args[1]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(get_directories)}");
            }
            var result = new TsObject[directories.Length];
            for (var i = 0; i < directories.Length; i++)
                result[i] = new DirectoryInfo(directories[i]);

            return result;
        }

        /// <summary>
        /// Gets an array of the files in the directory.
        /// </summary>
        /// <arg name="[search_pattern]" type="string">The search string to compare against the names of files. Can contain literal path characters and the wildcards * and ?</arg>
        /// <arg name="[search_option=SearchOption.TopDirectoryOnly]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption)">Specifies whether the search operation should only include the current directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.getfiles?view=netframework-4.7</source>
        /// <returns>array</returns>
        public TsObject get_files(TsObject[] args)
        {
            System.IO.FileInfo[] files;
            switch (args?.Length)
            {
                case null:
                case 0:
                    files = Source.GetFiles();
                    break;
                case 1:
                    files = Source.GetFiles((string)args[0]);
                    break;
                case 2:
                    files = Source.GetFiles((string)args[0], (SearchOption)(int)args[1]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(get_files)}");
            }
            var result = new TsObject[files.Length];
            for (var i = 0; i < files.Length; i++)
                result[i] = new FileInfo(files[i]);

            return result;
        }

        /// <summary>
        /// Moves this directory and its contents to a new path.
        /// </summary>
        /// <arg name="destinationPath" type="string">The path to move this directory to.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.moveto?view=netframework-4.7</source>
        /// <returns>null</returns>
        public TsObject move_to(TsObject[] args)
        {
            Source.MoveTo((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Refreshes the state of the directory.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.refresh</source>
        /// <returns>null</returns>
        public TsObject refresh(TsObject[] args)
        {
            Source.Refresh();
            return TsObject.Empty;
        }

        public static implicit operator TsObject(DirectoryInfo info) => new TsInstanceWrapper(info);
        public static explicit operator DirectoryInfo(TsObject obj) => (DirectoryInfo)obj.WeakValue;
    }
}
