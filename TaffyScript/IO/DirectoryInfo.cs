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
    [TaffyScriptObject]
    public class DirectoryInfo : ITsInstance
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

        public TsObject create(TsObject[] args)
        {
            Source.Create();
            return TsObject.Empty;
        }

        public TsObject create_subdirectory(TsObject[] args)
        {
            return new DirectoryInfo(Source.CreateSubdirectory((string)args[0]));
        }

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

        public TsObject get_directories(TsObject[] args)
        {
            if (args is null)
                return new TsList(Source.EnumerateDirectories().Select(di => (TsObject)new DirectoryInfo(di)));

            switch (args.Length)
            {
                case 0:
                    return new TsList(Source.EnumerateDirectories().Select(di => (TsObject)new DirectoryInfo(di)));
                case 1:
                    return new TsList(Source.EnumerateDirectories((string)args[0]).Select(di => (TsObject)new DirectoryInfo(di)));
                case 2:
                    return new TsList(Source.EnumerateDirectories((string)args[0], (SearchOption)(int)args[1]).Select(di => (TsObject)new DirectoryInfo(di)));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(get_directories)}");
            }
        }

        public TsObject get_files(TsObject[] args)
        {
            if (args is null)
                return new TsList(Source.EnumerateFiles().Select(fi => (TsObject)new FileInfo(fi)));

            switch (args.Length)
            {
                case 0:
                    return new TsList(Source.EnumerateFiles().Select(fi => (TsObject)new FileInfo(fi)));
                case 1:
                    return new TsList(Source.EnumerateFiles((string)args[0]).Select(fi => (TsObject)new FileInfo(fi)));
                case 2:
                    return new TsList(Source.EnumerateFiles((string)args[0], (SearchOption)(int)args[1]).Select(fi => (TsObject)new FileInfo(fi)));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(get_files)}");
            }
        }

        public TsObject move_to(TsObject[] args)
        {
            Source.MoveTo((string)args[0]);
            return TsObject.Empty;
        }

        public TsObject refresh(TsObject[] args)
        {
            Source.Refresh();
            return TsObject.Empty;
        }

        public static implicit operator TsObject(DirectoryInfo info) => new TsInstanceWrapper(info);
        public static explicit operator DirectoryInfo(TsObject obj) => (DirectoryInfo)obj.WeakValue;
    }
}
