using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript.IO
{
    /// <summary>
    /// Provides access to scripts and objects related to processing input and output.
    /// </summary>
    [TaffyScriptBaseType]
    public static class IOMethods
    {
        #region Directory

        /// <summary>
        /// Creates a directory and all subdirectories in the specified path unless they already exist.
        /// </summary>
        /// <arg name="path" type="string">The path of the directories to try and create.</arg>
        /// <returns>[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)</returns>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.createdirectory?view=netframework-4.7</source>
        public static TsObject directory_create(TsObject[] args)
        {
            return new DirectoryInfo(Directory.CreateDirectory((string)args[0]));
        }

        /// <summary>
        /// Deletes the directory, and, if specified, any subdirectories and files.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory to delete.</arg>
        /// <arg name="[recursive=false]" type="bool">Determines whether to delete files and subdirectories. Throws an exception if this is false but the specified directory isn't empty.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.delete?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_delete(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    Directory.Delete((string)args[0]);
                    break;
                case 2:
                    Directory.Delete((string)args[0], (bool)args[1]);
                    break;
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Gets a collection of directory names that meet the specified criteria.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory to search.</arg>
        /// <arg name="[search_pattern]" type="string">The string to match against names of the directories in the specified path. Can include path literals and the wildcards * and ?</arg>
        /// <arg name="[search_option]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.7)">Specifies whether the search operation should only include the top directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.enumeratedirectories?view=netframework-4.7</source>
        /// <returns>Enumerable</returns>
        public static TsObject directory_enumerate_directories(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new WrappedEnumerable(Directory.EnumerateDirectories((string)args[0]).Select(d => (TsObject)d));
                case 2:
                    return new WrappedEnumerable(Directory.EnumerateDirectories((string)args[0], (string)args[1]).Select(d => (TsObject)d));
                case 3:
                    return new WrappedEnumerable(Directory.EnumerateDirectories((string)args[0], (string)args[1], (SearchOption)(int)args[2]).Select(d => (TsObject)d));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(directory_enumerate_directories)}");
            }
        }

        /// <summary>
        /// Gets a collection of file names that meet the specified criteria.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory to search.</arg>
        /// <arg name="[search_pattern]" type="string">The string to match against names of the files in the specified path. Can include path literals and the wildcards * and ?</arg>
        /// <arg name="[search_option]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.7)">Specifies whether the search operation should only include files in the top directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.enumeratefiles?view=netframework-4.7</source>
        /// <returns>Enumerable</returns>
        public static TsObject directory_enumerate_files(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new WrappedEnumerable(Directory.EnumerateFiles((string)args[0]).Select(d => (TsObject)d));
                case 2:
                    return new WrappedEnumerable(Directory.EnumerateFiles((string)args[0], (string)args[1]).Select(d => (TsObject)d));
                case 3:
                    return new WrappedEnumerable(Directory.EnumerateFiles((string)args[0], (string)args[1], (SearchOption)(int)args[2]).Select(d => (TsObject)d));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(directory_enumerate_directories)}");
            }
        }

        /// <summary>
        /// Gets a collection of file and directory names that meet the specified criteria.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory to search.</arg>
        /// <arg name="[search_pattern]" type="string">The string to match against names in the specified path. Can include path literals and the wildcards * and ?</arg>
        /// <arg name="[search_option]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.7)">Specifies whether the search operation should only include files and directories in the top directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.enumeratefilesystementries?view=netframework-4.7</source>
        /// <returns>Enumerable</returns>
        public static TsObject directory_enumerate_file_system_entries(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new WrappedEnumerable(Directory.EnumerateFileSystemEntries((string)args[0]).Select(d => (TsObject)d));
                case 2:
                    return new WrappedEnumerable(Directory.EnumerateFileSystemEntries((string)args[0], (string)args[1]).Select(d => (TsObject)d));
                case 3:
                    return new WrappedEnumerable(Directory.EnumerateFileSystemEntries((string)args[0], (string)args[1], (SearchOption)(int)args[2]).Select(d => (TsObject)d));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(directory_enumerate_directories)}");
            }
        }

        /// <summary>
        /// Determines if a diretory with the specified path exists.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory to test.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.exists?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public static TsObject directory_exists(TsObject[] args)
        {
            return Directory.Exists((string)args[0]);
        }

        /// <summary>
        /// Gets the creation time of a directory.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getcreationtime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject directory_get_creation_time(TsObject[] args)
        {
            return new TsDateTime(Directory.GetCreationTime((string)args[0]));
        }

        /// <summary>
        /// Gets the creation time of a directory in universal coordinated time.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getcreationtimeutc?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject directory_get_creation_time_utc(TsObject[] args)
        {
            return new TsDateTime(Directory.GetCreationTimeUtc((string)args[0]));
        }

        /// <summary>
        /// Gets the working directory of the application.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getcurrentdirectory?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject directory_get_current(TsObject[] args)
        {
            return Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Gets an array of directory names that meet the specified criteria.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory to search.</arg>
        /// <arg name="[search_pattern]" type="string">The string to match against names of the directories in the specified path. Can include path literals and the wildcards * and ?</arg>
        /// <arg name="[search_option]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.7)">Specifies whether the search operation should only include the top directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getdirectories?view=netframework-4.7</source>
        /// <returns>array</returns>
        public static TsObject directory_get_directories(TsObject[] args)
        {
            string[] directories;
            switch (args.Length)
            {
                case 1:
                    directories = Directory.GetDirectories((string)args[0]);
                    break;
                case 2:
                    directories = Directory.GetDirectories((string)args[0], (string)args[1]);
                    break;
                case 3:
                    directories = Directory.GetDirectories((string)args[0], (string)args[1], (SearchOption)(int)args[2]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(directory_get_directories)}");
            }
            var result = new TsObject[directories.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = directories[i];

            return result;
        }

        /// <summary>
        /// Gets an array of file names that meet the specified criteria.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory to search.</arg>
        /// <arg name="[search_pattern]" type="string">The string to match against names of the files in the specified path. Can include path literals and the wildcards * and ?</arg>
        /// <arg name="[search_option]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.7)">Specifies whether the search operation should only include files in the top directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?view=netframework-4.7</source>
        /// <returns>array</returns>
        public static TsObject directory_get_files(TsObject[] args)
        {
            string[] files;
            switch (args.Length)
            {
                case 1:
                    files = Directory.GetFiles((string)args[0]);
                    break;
                case 2:
                    files = Directory.GetFiles((string)args[0], (string)args[1]);
                    break;
                case 3:
                    files = Directory.GetFiles((string)args[0], (string)args[1], (SearchOption)(int)args[2]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(directory_get_directories)}");
            }
            var result = new TsObject[files.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = files[i];

            return result;
        }

        /// <summary>
        /// Gets an array of file and directory names that meet the specified criteria.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory to search.</arg>
        /// <arg name="[search_pattern]" type="string">The string to match against names in the specified path. Can include path literals and the wildcards * and ?</arg>
        /// <arg name="[search_option]" type="[SearchOption](https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.7)">Specifies whether the search operation should only include files and directories in the top directory or all subdirectories.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getfilesystementries?view=netframework-4.7</source>
        /// <returns>array</returns>
        public static TsObject directory_get_file_system_entries(TsObject[] args)
        {
            string[] entries;
            switch (args.Length)
            {
                case 1:
                    entries = Directory.GetFileSystemEntries((string)args[0]);
                    break;
                case 2:
                    entries = Directory.GetFiles((string)args[0], (string)args[1]);
                    break;
                case 3:
                    entries = Directory.GetFiles((string)args[0], (string)args[1], (SearchOption)(int)args[2]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(directory_get_directories)}");
            }
            var result = new TsObject[entries.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = entries[i];

            return result;
        }

        /// <summary>
        /// Gets the last access time of a directory.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getlastaccesstime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject directory_get_last_access_time(TsObject[] args)
        {
            return new TsDateTime(Directory.GetLastAccessTime((string)args[0]));
        }

        /// <summary>
        /// Gets the last access time of a directory in universal coordinated time.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getlastaccesstimeutc?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject directory_get_last_access_time_utc(TsObject[] args)
        {
            return new TsDateTime(Directory.GetLastAccessTimeUtc((string)args[0]));
        }

        /// <summary>
        /// Gets the last write time of a directory.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getlastwritetime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject directory_get_last_write_time(TsObject[] args)
        {
            return new TsDateTime(Directory.GetLastWriteTime((string)args[0]));
        }

        /// <summary>
        /// Gets the last write time of a directory in universal coordinated time.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getlastaccesstimeutc?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject directory_get_last_write_time_utc(TsObject[] args)
        {
            return new TsDateTime(Directory.GetLastWriteTimeUtc((string)args[0]));
        }

        /// <summary>
        /// Gets a list of logical drives on this computer.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getlogicaldrives?view=netframework-4.7</source>
        /// <returns>[List]({{site.baseurl}}/docs/List)</returns>
        public static TsObject directory_get_logical_drives(TsObject[] args)
        {
            return new TsList(Directory.GetLogicalDrives().Select(drive => (TsObject)drive));
        }

        /// <summary>
        /// Gets the volume/root information of a path.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getdirectoryroot?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject directory_get_root(TsObject[] args)
        {
            return Directory.GetDirectoryRoot((string)args[0]);
        }

        /// <summary>
        /// Gets the parent directory of a directory.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getparent?view=netframework-4.7</source>
        /// <returns>[DirectoryInfo]({{site.baseurl}}/docs/TaffyScript/IO/DirectoryInfo)</returns>
        public static TsObject directory_get_parent(TsObject[] args)
        {
            return new DirectoryInfo(Directory.GetParent((string)args[0]));
        }

        /// <summary>
        /// Moves a directory and its contents to a new location.
        /// </summary>
        /// <arg name="source_directory" type="string">The path of the source directory.</arg>
        /// <arg name="destination_directory" type="string">The path of the destination directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.move?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_move(TsObject[] args)
        {
            Directory.Move((string)args[0], (string)args[1]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the creation time of a directory.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time the directory was created.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setcreationtime?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_set_creation_time(TsObject[] args)
        {
            Directory.SetCreationTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the creation time of a directory in universal coordinated time.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time the directory was created.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setcreationtimeutc?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_set_creation_time_utc(TsObject[] args)
        {
            Directory.SetCreationTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the working directory of the application.
        /// </summary>
        /// <arg name="path" type="string">The path to set the working directory to.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setcurrentdirectory?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_set_current(TsObject[] args)
        {
            Directory.SetCurrentDirectory((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the last access time of a directory.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time the directory was last accessed.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setlastaccesstime?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_set_last_access_time(TsObject[] args)
        {
            Directory.SetLastAccessTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the last access time of a directory in universal coordinated time.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time the directory was last accessed.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setlastaccesstimeutc?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_set_last_access_utc(TsObject[] args)
        {
            Directory.SetLastAccessTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the last write time of a directory.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time the directory was last written to.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setlastwritetime?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_set_last_write_time(TsObject[] args)
        {
            Directory.SetLastWriteTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the last write time of a directory in universal coordinated time.
        /// </summary>
        /// <arg name="path" type="string">The path of the directory.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time the directory was last written to.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setlastwritetimeutc?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject directory_set_last_write_time_utc(TsObject[] args)
        {
            Directory.SetLastWriteTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        #endregion

        #region File

        /// <summary>
        /// Appends the lines to a file and then closes the file. If the file does not exist, it is created first.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to write to.</arg>
        /// <arg name="lines" type="array or [Enumerable]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable)">The lines to write to the file.</arg>
        /// <arg name="[encoding]" type="string">The name of the encoding to use to write the lines. Defaults to utf-8.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.appendalllines?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_append_all_lines(TsObject[] args)
        {
            var path = (string)args[0];
            IEnumerable<string> lines;
            if (args[1].Type == VariableType.Array)
                lines = args[1].GetArray().Select(o => (string)o);
            else
                lines = ((IEnumerable<TsObject>)args[0].WeakValue).Select(o => (string)o);

            switch(args.Length)
            {
                case 2:
                    File.AppendAllLines(path, lines);
                    break;
                case 3:
                    File.AppendAllLines(path, lines, Encoding.GetEncoding((string)args[2]));
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_append_all_lines)}");
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Appends the text to a file and then closes the file. If the file does not exist, it is created first.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to write to.</arg>
        /// <arg name="text" type="string">The text to write to the file.</arg>
        /// <arg name="[encoding]" type="string">The name of the encoding to use to write the lines. Defaults to utf-8.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.appendalltext?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_append_all_text(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    File.AppendAllText((string)args[0], (string)args[1]);
                    break;
                case 3:
                    File.AppendAllText((string)args[0], (string)args[1], Encoding.GetEncoding((string)args[2]));
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_append_all_text)}");
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Creates a [StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter) that appends utf-8 encoded text to the file. If the file does not exist, it is created first.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to write to.</arg>
        /// <returns>[StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter)</returns>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.appendtext?view=netframework-4.7</source>
        public static TsObject file_append_text(TsObject[] args)
        {
            return new StreamWriter(File.AppendText((string)args[0]));
        }

        /// <summary>
        /// Copies an existing file to a new file, optionally allowing the overwrite of an existing file.
        /// </summary>
        /// <arg name="source_file" type="string">The path of the source file.</arg>
        /// <arg name="dest_file" type="string">The path of the destination file.</arg>
        /// <arg name="[overwrite=false]">Determines whether this script is allowed to overwrite an existing file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.copy?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_copy(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    File.Copy((string)args[0], (string)args[1]);
                    break;
                case 3:
                    File.Copy((string)args[0], (string)args[1], (bool)args[2]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_copy)}");
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Creates a file with the specified path and returns a FileStream for it.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the new file.</arg>
        /// <arg name="[buffer_size]" type="number">The size of the FileStream buffer.</arg>
        /// <arg name="[options]" type="[FileOptions](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileoptions?view=netframework-4.7)">Describes how to create or overwrite the file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.create?view=netframework-4.7</source>
        /// <returns>[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)</returns>
        public static TsObject file_create(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new FileStream(File.Create((string)args[0]));
                case 2:
                    return new FileStream(File.Create((string)args[0], (int)args[1]));
                case 3:
                    return new FileStream(File.Create((string)args[0], (int)args[1], (FileOptions)(int)args[2]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_create)}");
            }
        }

        /// <summary>
        /// Creates or opens a file for writing utf-8 encoded text. If the file exists, its contents are overwritten.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to open.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.createtext?view=netframework-4.7</source>
        /// <returns>[StreamWriter]({{site.baseurl}}/docs/TaffyScript/IO/StreamWriter)</returns>
        public static TsObject file_create_text(TsObject[] args)
        {
            return new StreamWriter(File.CreateText((string)args[0]));
        }

        /// <summary>
        /// Decrypts a file that was encrypted by the current account.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to decrypt.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.decrypt?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_decrypt(TsObject[] args)
        {
            File.Decrypt((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to delete.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.delete?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_delete(TsObject[] args)
        {
            File.Delete((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Encrypts a file so that only the account used to encrypt it can decrypt it.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to encrypt.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.encrypt?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_encrypt(TsObject[] args)
        {
            File.Encrypt((string)args[0]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to check.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.exists?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public static TsObject file_exists(TsObject[] args)
        {
            return File.Exists((string)args[0]);
        }

        /// <summary>
        /// Gets the attributes of the specified file.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to get the attributes of.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.getattributes?view=netframework-4.7</source>
        /// <returns>[FileAttributes](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileattributes?view=netframework-4.7)</returns>
        public static TsObject file_get_attributes(TsObject[] args)
        {
            return (int)File.GetAttributes((string)args[0]);
        }

        /// <summary>
        /// Gets the creation time of the specified file.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to get the creation time.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.getcreationtime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject file_get_creation_time(TsObject[] args)
        {
            return new TsDateTime(File.GetCreationTime((string)args[0]));
        }

        /// <summary>
        /// Gets the creation time of the specified file in universal coordinated time.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to get the creation time.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.getcreationtimeutc?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject file_get_creation_time_utc(TsObject[] args)
        {
            return new TsDateTime(File.GetCreationTimeUtc((string)args[0]));
        }

        /// <summary>
        /// Gets the last access time of the specified file.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to get the last access time.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.getlastaccesstime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject file_get_last_access_time(TsObject[] args)
        {
            return new TsDateTime(File.GetLastAccessTime((string)args[0]));
        }

        /// <summary>
        /// Gets the last access time of the specified file in universal coordinated time.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to get the last access time.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.getlastaccesstimeutc?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject file_get_last_access_time_utc(TsObject[] args)
        {
            return new TsDateTime(File.GetLastAccessTimeUtc((string)args[0]));
        }

        /// <summary>
        /// Gets the last write time of the specified file.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to get the last write time.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.getlastwritetime?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject file_get_last_write_time(TsObject[] args)
        {
            return new TsDateTime(File.GetLastWriteTime((string)args[0]));
        }

        /// <summary>
        /// Gets the last write time of the specified file in universal coordinated time.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to get the last write time.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.getlastwritetimeutc?view=netframework-4.7</source>
        /// <returns>[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)</returns>
        public static TsObject file_get_last_write_time_utc(TsObject[] args)
        {
            return new TsDateTime(File.GetLastWriteTimeUtc((string)args[0]));
        }

        /// <summary>
        /// Moves the specified file to a new location.
        /// </summary>
        /// <arg name="source_file" type="string">The path of the source file.</arg>
        /// <arg name="dest_file" type="string">The path of the destination file.</arg>
        /// <source></source>
        /// <returns>null</returns>
        public static TsObject file_move(TsObject[] args)
        {
            File.Move((string)args[0], (string)args[1]);
            return TsObject.Empty;
        }

        /// <summary>
        /// Opens a FileStream on the specified path.
        /// </summary>
        /// <arg name="path" type="string">The file to open.</arg>
        /// <arg name="mode" type="[FileMode](https://docs.microsoft.com/en-us/dotnet/api/system.io.filemode?view=netframework-4.7)">Determines whether a new file is created if one doesn't exist and whether to keep or overwrite the contents of existing files.</arg>
        /// <arg name="[access]" type="[FileAccess](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileaccess?view=netframework-4.7)">Determines the types of operations that can be performed on the file.</arg>
        /// <arg name="[share]" type="[FileShare](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileshare?view=netframework-4.7)">Specifies the type of access other threads have to the file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.move?view=netframework-4.7</source>
        /// <returns>[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)</returns>
        public static TsObject file_open(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    return new FileStream(File.Open((string)args[0], (FileMode)(int)args[1]));
                case 3:
                    return new FileStream(File.Open((string)args[0], (FileMode)(int)args[1], (FileAccess)(int)args[2]));
                case 4:
                    return new FileStream(File.Open((string)args[0], (FileMode)(int)args[1], (FileAccess)(int)args[2], (FileShare)(int)args[3]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_open)}");
            }
        }

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        /// <arg name="path" type="string">The file to open.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.openread?view=netframework-4.7</source>
        /// <returns>[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)</returns>
        public static TsObject file_open_read(TsObject[] args)
        {
            return new FileStream(File.OpenRead((string)args[0]));
        }

        /// <summary>
        /// Opens an existing utf-8 encoded text file for reading.
        /// </summary>
        /// <arg name="path" type="string">The file to open.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.opentext?view=netframework-4.7</source>
        /// <returns>[StreamReader]({{site.baseurl}}/docs/TaffyScript/IO/StreamReader)</returns>
        public static TsObject file_open_text(TsObject[] args)
        {
            return new StreamReader(File.OpenText((string)args[0]));
        }

        /// <summary>
        /// Opens an existing file or creates a new file for writing.
        /// </summary>
        /// <arg name="path" type="string">The file to open or create.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.openwrite?view=netframework-4.7</source>
        /// <returns>[FileStream]({{site.baseurl}}/docs/TaffyScript/IO/FileStream)</returns>
        public static TsObject file_open_write(TsObject[] args)
        {
            return new FileStream(File.OpenWrite((string)args[0]));
        }

        /// <summary>
        /// Opens a binary file, reads the contents into a Buffer, and then closes the file.
        /// </summary>
        /// <arg name="path" type="string">The file to open.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readallbytes?view=netframework-4.7</source>
        /// <returns>[Buffer]({{site.baseurl}}/docs/TaffyScript/IO/Buffer)</returns>
        public static TsObject file_read_all_bytes(TsObject[] args)
        {
            return new Buffer(File.ReadAllBytes((string)args[0]));
        }

        /// <summary>
        /// Opens a text file, reads all lines into an array, then closes the file.
        /// </summary>
        /// <arg name="path" type="string">The file to open.</arg>
        /// <arg name="[encoding=*utf-8*]" type="string">The name of the encoding used to read the file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readalllines?view=netframework-4.7</source>
        /// <returns>array</returns>
        public static TsObject file_read_all_lines(TsObject[] args)
        {
            string[] lines;
            switch(args.Length)
            {
                case 1:
                    lines = File.ReadAllLines((string)args[0]);
                    break;
                case 2:
                    lines = File.ReadAllLines((string)args[0], Encoding.GetEncoding((string)args[1]));
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_read_all_lines)}");
            }

            var result = new TsObject[lines.Length];
            for (var i = 0; i < lines.Length; i++)
                result[i] = lines[i];

            return result;
        }

        /// <summary>
        /// Opens a text file, reads the contents into a string, then closes the file.
        /// </summary>
        /// <arg name="path" type="string">The file to open.</arg>
        /// <arg name="[encoding=*utf-8*]" type="string">The name of the encoding used to read the file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readalltext?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject file_read_all_text(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new TsList(File.ReadAllText((string)args[0]).Select(s => (TsObject)s));
                case 2:
                    return new TsList(File.ReadAllText((string)args[0], Encoding.GetEncoding((string)args[1])).Select(s => (TsObject)s));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_read_all_text)}");
            }
        }

        /// <summary>
        /// Reads the lines of a file.
        /// </summary>
        /// <arg name="path" type="string">The file to open.</arg>
        /// <arg name="[encoding=*utf-8*]" type="string">The name of the encoding used to read the file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readlines?view=netframework-4.7</source>
        /// <returns>[Enumerable]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable)</returns>
        public static TsObject file_read_lines(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new WrappedEnumerable(File.ReadLines((string)args[0]).Select(s => (TsObject)s));
                case 2:
                    return new WrappedEnumerable(File.ReadLines((string)args[0], Encoding.GetEncoding((string)args[1])).Select(s => (TsObject)s));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_read_lines)}");
            }
        }

        /// <summary>
        /// Replaces the contents of a file with the contents of another file, deleting the original file and creating a backup of the replaced file.
        /// </summary>
        /// <arg name="source_file" type="string">The path of the source file.</arg>
        /// <arg name="dest_file" type="string">The path of the file to be replaced.</arg>
        /// <arg name="dest_backup_file" type="string">The path of the backup file.</arg>
        /// <arg name="[ignore_metadata_errors=false]" type="bool">Determines whether to ignore merge errors from the replaced file to the replacement file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.replace?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_replace(TsObject[] args)
        {
            switch(args.Length)
            {
                case 3:
                    File.Replace((string)args[0], (string)args[1], (string)args[2]);
                    break;
                case 4:
                    File.Replace((string)args[0], (string)args[1], (string)args[2], (bool)args[3]);
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_replace)}");
            }
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the creation time of the specified file.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to set the creation time.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time of the files creation.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.setcreationtime?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_set_creation_time(TsObject[] args)
        {
            File.SetCreationTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the creation time of the specified file in universal coordinated time.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to set the creation time.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time of the files creation.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.setcreationtimeutc?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_set_creation_time_utc(TsObject[] args)
        {
            File.SetCreationTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the last access time of the specified file.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to set the last access time.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time of the files last access.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.setlastaccesstime?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_set_last_access_time(TsObject[] args)
        {
            File.SetLastAccessTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the last access time of the specified file in universal coordinated time.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to set the last access time.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time of the files last access.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.setlastaccesstimeutc?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_set_last_access_time_utc(TsObject[] args)
        {
            File.SetLastAccessTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the last write time of the specified file.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to set the last write time.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time of the files last write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.setlastwritetime?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_set_last_write_time(TsObject[] args)
        {
            File.SetLastWriteTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Sets the last write time of the specified file in universal coordinated time.
        /// </summary>
        /// <arg name="file_name" type="string">The path of the file to set the last write time.</arg>
        /// <arg name="time" type="[DateTime]({{site.baseurl}}/docs/TaffyScript/DateTime)">The date and time of the files last write.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.setlastwritetimeutc?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_set_last_write_time_utc(TsObject[] args)
        {
            File.SetLastWriteTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        /// <summary>
        /// Creates a new file, writes the specified Buffer to the file, then closes the file. If the files exists, it is overwritten.
        /// </summary>
        /// <arg name="path" type="string">The path of the file to write.</arg>
        /// <arg name="buffer" type="[Buffer]({{site.baseurl}}/docs/TaffyScript/IO/Buffer)">The buffer to write to the file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.writeallbytes?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_write_all_bytes(TsObject[] args)
        {
            File.WriteAllBytes((string)args[0], ((Buffer)args[1]).Memory);
            return TsObject.Empty;
        }

        /// <summary>
        /// Creates a new file, writes one or more strings to the file, then closes the file.
        /// </summary>
        /// <arg name="path" type="string">The path of the file to create.</arg>
        /// <arg name="lines" type="[Enumerable]({{site.baseurl}}/docs/TaffyScript/Collections/Enumerable)">The lines to write to the file.</arg>
        /// <arg name="[encoding=*utf-8*]" type="string">The name of the encoding used to write to the file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.writealllines?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_write_all_lines(TsObject[] args)
        {
            IEnumerable<string> lines;
            if (args[1].Type == VariableType.Array)
                lines = args[1].GetArray().Select(o => (string)o);
            else
                lines = ((IEnumerable<TsObject>)args[0].WeakValue).Select(o => (string)o);

            switch (args.Length)
            {
                case 2:
                    File.WriteAllLines((string)args[0], lines);
                    break;
                case 3:
                    File.WriteAllLines((string)args[0], lines, Encoding.GetEncoding((string)args[2]));
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_write_all_lines)}");
            }

            return TsObject.Empty;
        }

        /// <summary>
        /// Creates a new file, writes the contents to the file, then closes the file.
        /// </summary>
        /// <arg name="path" type="string">The path of the file to create.</arg>
        /// <arg name="content" type="string">The content to write to the file.</arg>
        /// <arg name="[encoding=*utf-8*]" type="string">The name of the encoding used to write to the file.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=netframework-4.7</source>
        /// <returns>null</returns>
        public static TsObject file_write_all_text(TsObject[] args)
        {
            switch (args.Length)
            {
                case 2:
                    File.WriteAllText((string)args[0], (string)args[1]);
                    break;
                case 3:
                    File.WriteAllText((string)args[0], (string)args[1], Encoding.GetEncoding((string)args[2]));
                    break;
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_write_all_text)}");
            }

            return TsObject.Empty;
        }

        #endregion

        #region Path

        /// <summary>
        /// Changes the extension of a path.
        /// </summary>
        /// <arg name="path" type="string">The path to modify.</arg>
        /// <arg name="extension" type="string">The new extension (with or without a leading period). Use null to remove an existing extension.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.changeextension?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_change_extension(TsObject[] args)
        {
            return Path.ChangeExtension((string)args[0], (string)args[1]);
        }

        /// <summary>
        /// Combines the arguments into a single path.
        /// </summary>
        /// <arg name="..paths" type="strings">The path strings to combine together.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_combine(TsObject[] args)
        {
            switch(args.Length)
            {
                case 2:
                    return Path.Combine((string)args[0], (string)args[1]);
                case 3:
                    return Path.Combine((string)args[0], (string)args[1], (string)args[2]);
                case 4:
                    return Path.Combine((string)args[0], (string)args[1], (string)args[2], (string)args[3]);
                default:
                    if (args.Length < 2)
                        throw new ArgumentException($"Invalid number of arguments passed to {nameof(path_combine)}");

                    var parts = new string[args.Length];
                    for (var i = 0; i < args.Length; i++)
                        parts[i] = (string)args[i];
                    return Path.Combine(parts);
            }
        }

        /// <summary>
        /// Gets the directory name for the specified path.
        /// </summary>
        /// <arg name="path" type="string">The path of a file or directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getdirectoryname?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_directory_name(TsObject[] args)
        {
            return Path.GetDirectoryName((string)args[0]);
        }

        /// <summary>
        /// Gets the extension of the specified path.
        /// </summary>
        /// <arg name="path" type="string">The path of a file or directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getextension?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_extension(TsObject[] args)
        {
            return Path.GetExtension((string)args[0]);
        }

        /// <summary>
        /// Gets the file name and extension of the the specified path.
        /// </summary>
        /// <arg name="path" type="string">The path of a file or directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getfilename?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_file_name(TsObject[] args)
        {
            return Path.GetFileName((string)args[0]);
        }

        /// <summary>
        /// Gets the file name without the extension of the the specified path.
        /// </summary>
        /// <arg name="path" type="string">The path of a file or directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getfilenamewithoutextension?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_file_name_without_extension(TsObject[] args)
        {
            return Path.GetFileNameWithoutExtension((string)args[0]);
        }

        /// <summary>
        /// Gets the absolute path for the specified path.
        /// </summary>
        /// <arg name="path" type="string">The path of a file or directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getfullpath?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_full(TsObject[] args)
        {
            return Path.GetFullPath((string)args[0]);
        }

        /// <summary>
        /// Gets an array containing the invalid characters that are not allowed in file names.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getinvalidfilenamechars?view=netframework-4.7</source>
        /// <returns>array</returns>
        public static TsObject path_get_invalid_file_name_chars(TsObject[] args)
        {
            var chars = Path.GetInvalidFileNameChars();
            var result = new TsObject[chars.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = chars[i];
            return result;
        }

        /// <summary>
        /// Gets an array containing the invalid characters that are not allowed in path names.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getinvalidpathchars?view=netframework-4.7</source>
        /// <returns>array</returns>
        public static TsObject path_get_invalid_chars(TsObject[] args)
        {
            var chars = Path.GetInvalidPathChars();
            var result = new TsObject[chars.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = chars[i];
            return result;
        }

        /// <summary>
        /// Gets the root directory of the specified path.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getpathroot?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_root(TsObject[] args)
        {
            return Path.GetPathRoot((string)args[0]);
        }

        /// <summary>
        /// Gets a random folder or file name.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getrandomfilename?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_random_file_name(TsObject[] args)
        {
            return Path.GetRandomFileName();
        }

        /// <summary>
        /// Creates a uniquely named temporary file on disk and returns the full path of that file.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.gettempfilename?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_temp_file_name(TsObject[] args)
        {
            return Path.GetTempFileName();
        }

        /// <summary>
        /// Gets the path to the users temporary folder.
        /// </summary>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.gettemppath?view=netframework-4.7</source>
        /// <returns>string</returns>
        public static TsObject path_get_temp(TsObject[] args)
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// Determines whether a path includes a file name extension.
        /// </summary>
        /// <arg name="path" type="string">The path of a file or directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.hasextension?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public static TsObject path_has_extension(TsObject[] args)
        {
            return Path.HasExtension((string)args[0]);
        }

        /// <summary>
        /// Determines if the specified path contains a root.
        /// </summary>
        /// <arg name="path" type="string">The path of a file or directory.</arg>
        /// <source>https://docs.microsoft.com/en-us/dotnet/api/system.io.path.ispathrooted?view=netframework-4.7</source>
        /// <returns>bool</returns>
        public static TsObject path_is_rooted(TsObject[] args)
        {
            return Path.IsPathRooted((string)args[0]);
        } 

        #endregion
    }
}
