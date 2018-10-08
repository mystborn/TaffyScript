using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaffyScript.Collections;

namespace TaffyScript.IO
{
    [TaffyScriptBaseType]
    public static class IOMethods
    {
        #region Directory

        public static TsObject directory_create(TsObject[] args)
        {
            return new DirectoryInfo(Directory.CreateDirectory((string)args[0]));
        }

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

        public static TsObject directory_exists(TsObject[] args)
        {
            return Directory.Exists((string)args[0]);
        }

        public static TsObject directory_get_creation_time(TsObject[] args)
        {
            return new TsDateTime(Directory.GetCreationTime((string)args[0]));
        }

        public static TsObject directory_get_creation_time_utc(TsObject[] args)
        {
            return new TsDateTime(Directory.GetCreationTimeUtc((string)args[0]));
        }

        public static TsObject directory_get_current(TsObject[] args)
        {
            return Directory.GetCurrentDirectory();
        }

        public static TsObject directory_get_directories(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new TsList(Directory.EnumerateDirectories((string)args[0]).Select(p => (TsObject)p));
                case 2:
                    return new TsList(Directory.EnumerateDirectories((string)args[0], (string)args[1]).Select(p => (TsObject)p));
                case 3:
                    return new TsList(Directory.EnumerateDirectories((string)args[0], (string)args[1], (SearchOption)(int)args[2]).Select(p => (TsObject)p));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(directory_get_directories)}");
            }
        }

        public static TsObject directory_get_files(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new TsList(Directory.EnumerateFiles((string)args[0]).Select(p => (TsObject)p));
                case 2:
                    return new TsList(Directory.EnumerateFiles((string)args[0], (string)args[1]).Select(p => (TsObject)p));
                case 3:
                    return new TsList(Directory.EnumerateFiles((string)args[0], (string)args[1], (SearchOption)(int)args[2]).Select(p => (TsObject)p));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(directory_get_files)}");
            }
        }

        public static TsObject directory_get_last_access_time(TsObject[] args)
        {
            return new TsDateTime(Directory.GetLastAccessTime((string)args[0]));
        }

        public static TsObject directory_get_last_access_time_utc(TsObject[] args)
        {
            return new TsDateTime(Directory.GetLastAccessTimeUtc((string)args[0]));
        }

        public static TsObject directory_get_last_write_time(TsObject[] args)
        {
            return new TsDateTime(Directory.GetLastWriteTime((string)args[0]));
        }

        public static TsObject directory_get_last_write_time_utc(TsObject[] args)
        {
            return new TsDateTime(Directory.GetLastWriteTimeUtc((string)args[0]));
        }

        public static TsObject directory_get_logical_drives(TsObject[] args)
        {
            return new TsList(Directory.GetLogicalDrives().Select(drive => (TsObject)drive));
        }

        public static TsObject directory_get_root(TsObject[] args)
        {
            return Directory.GetDirectoryRoot((string)args[0]);
        }

        public static TsObject directory_get_parent(TsObject[] args)
        {
            return new DirectoryInfo(Directory.GetParent((string)args[0]));
        }

        public static TsObject directory_move(TsObject[] args)
        {
            Directory.Move((string)args[0], (string)args[1]);
            return TsObject.Empty;
        }

        public static TsObject directory_set_creation_time(TsObject[] args)
        {
            Directory.SetCreationTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject directory_set_creation_time_utc(TsObject[] args)
        {
            Directory.SetCreationTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject directory_set_current(TsObject[] args)
        {
            Directory.SetCurrentDirectory((string)args[0]);
            return TsObject.Empty;
        }

        public static TsObject directory_set_last_access_time(TsObject[] args)
        {
            Directory.SetLastAccessTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject directory_set_last_access_utc(TsObject[] args)
        {
            Directory.SetLastAccessTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject directory_set_last_write_time(TsObject[] args)
        {
            Directory.SetLastWriteTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject directory_set_last_write_time_utc(TsObject[] args)
        {
            Directory.SetLastWriteTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        #endregion

        #region File

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

        public static TsObject file_append_text(TsObject[] args)
        {
            return new StreamWriter(File.AppendText((string)args[0]));
        }

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

        public static TsObject file_create_text(TsObject[] args)
        {
            return new StreamWriter(File.CreateText((string)args[0]));
        }

        public static TsObject file_decrypt(TsObject[] args)
        {
            File.Decrypt((string)args[0]);
            return TsObject.Empty;
        }

        public static TsObject file_delete(TsObject[] args)
        {
            File.Delete((string)args[0]);
            return TsObject.Empty;
        }

        public static TsObject file_encrypt(TsObject[] args)
        {
            File.Encrypt((string)args[0]);
            return TsObject.Empty;
        }

        public static TsObject file_exists(TsObject[] args)
        {
            return File.Exists((string)args[0]);
        }

        public static TsObject file_get_attributes(TsObject[] args)
        {
            return (int)File.GetAttributes((string)args[0]);
        }

        public static TsObject file_get_creation_time(TsObject[] args)
        {
            return new TsDateTime(File.GetCreationTime((string)args[0]));
        }

        public static TsObject file_get_creation_time_utc(TsObject[] args)
        {
            return new TsDateTime(File.GetCreationTimeUtc((string)args[0]));
        }

        public static TsObject file_get_last_access_time(TsObject[] args)
        {
            return new TsDateTime(File.GetLastAccessTime((string)args[0]));
        }

        public static TsObject file_get_last_access_time_utc(TsObject[] args)
        {
            return new TsDateTime(File.GetLastAccessTimeUtc((string)args[0]));
        }

        public static TsObject file_get_last_write_time(TsObject[] args)
        {
            return new TsDateTime(File.GetLastWriteTime((string)args[0]));
        }

        public static TsObject file_get_last_write_time_utc(TsObject[] args)
        {
            return new TsDateTime(File.GetLastWriteTimeUtc((string)args[0]));
        }

        public static TsObject file_move(TsObject[] args)
        {
            File.Move((string)args[0], (string)args[1]);
            return TsObject.Empty;
        }

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

        public static TsObject file_open_read(TsObject[] args)
        {
            return new FileStream(File.OpenRead((string)args[0]));
        }

        public static TsObject file_open_text(TsObject[] args)
        {
            return new StreamReader(File.OpenText((string)args[0]));
        }

        public static TsObject file_open_write(TsObject[] args)
        {
            return new FileStream(File.OpenWrite((string)args[0]));
        }

        public static TsObject file_read_all_lines(TsObject[] args)
        {
            switch(args.Length)
            {
                case 1:
                    return new TsList(File.ReadAllLines((string)args[0]).Select(s => (TsObject)s));
                case 2:
                    return new TsList(File.ReadAllLines((string)args[0], Encoding.GetEncoding((string)args[1])).Select(s => (TsObject)s));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {nameof(file_read_all_lines)}");
            }
        }

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

        public static TsObject file_set_creation_time(TsObject[] args)
        {
            File.SetCreationTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject file_set_creation_time_utc(TsObject[] args)
        {
            File.SetCreationTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject file_set_last_access_time(TsObject[] args)
        {
            File.SetLastAccessTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject file_set_last_access_time_utc(TsObject[] args)
        {
            File.SetLastAccessTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject file_set_last_write_time(TsObject[] args)
        {
            File.SetLastWriteTime((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

        public static TsObject file_set_last_write_time_utc(TsObject[] args)
        {
            File.SetLastWriteTimeUtc((string)args[0], ((TsDateTime)args[1]).Source);
            return TsObject.Empty;
        }

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

        public static TsObject path_change_extension(TsObject[] args)
        {
            return Path.ChangeExtension((string)args[0], (string)args[1]);
        }

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

        public static TsObject path_get_directory_name(TsObject[] args)
        {
            return Path.GetDirectoryName((string)args[0]);
        }

        public static TsObject path_get_extension(TsObject[] args)
        {
            return Path.GetExtension((string)args[0]);
        }

        public static TsObject path_get_file_name(TsObject[] args)
        {
            return Path.GetFileName((string)args[0]);
        }

        public static TsObject path_get_file_name_without_extension(TsObject[] args)
        {
            return Path.GetFileNameWithoutExtension((string)args[0]);
        }

        public static TsObject path_get_full(TsObject[] args)
        {
            return Path.GetFullPath((string)args[0]);
        }

        public static TsObject path_get_invalid_file_name_chars(TsObject[] args)
        {
            var chars = Path.GetInvalidFileNameChars();
            var result = new TsObject[chars.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = chars[i];
            return result;
        }

        public static TsObject path_get_invalid_chars(TsObject[] args)
        {
            var chars = Path.GetInvalidPathChars();
            var result = new TsObject[chars.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = chars[i];
            return result;
        }

        public static TsObject path_get_root(TsObject[] args)
        {
            return Path.GetPathRoot((string)args[0]);
        }

        public static TsObject path_get_random_file_name(TsObject[] args)
        {
            return Path.GetRandomFileName();
        }

        public static TsObject path_get_temp_file_name(TsObject[] args)
        {
            return Path.GetTempFileName();
        }

        public static TsObject path_get_temp(TsObject[] args)
        {
            return Path.GetTempPath();
        }

        public static TsObject path_has_extension(TsObject[] args)
        {
            return Path.HasExtension((string)args[0]);
        }

        public static TsObject path_is_rooted(TsObject[] args)
        {
            return Path.IsPathRooted((string)args[0]);
        } 

        #endregion
    }
}
