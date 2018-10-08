using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalSource = System.IO.FileInfo;

namespace TaffyScript.IO
{
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

        public TsObject append_text(TsObject[] args)
        {
            return new StreamWriter(Source.AppendText());
        }

        public TsObject copy_to(TsObject[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new FileInfo(Source.CopyTo((string)args[0]));
                case 2:
                    return new FileInfo(Source.CopyTo((string)args[1]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(copy_to)}");
            }
        }

        public TsObject create(TsObject[] args)
        {
            return new FileStream(Source.Create());
        }

        public TsObject create_text(TsObject[] args)
        {
            return new StreamWriter(Source.CreateText());
        }

        public TsObject decrypt(TsObject[] args)
        {
            Source.Decrypt();
            return TsObject.Empty;
        }

        public TsObject delete(TsObject[] args)
        {
            Source.Delete();
            return TsObject.Empty;
        }

        public TsObject encrypt(TsObject[] args)
        {
            Source.Encrypt();
            return TsObject.Empty;
        }

        public TsObject move_to(TsObject[] args)
        {
            Source.MoveTo((string)args[0]);
            return TsObject.Empty;
        }

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

        public TsObject open_read(TsObject[] args)
        {
            return new FileStream(Source.OpenRead());
        }

        public TsObject open_text(TsObject[] args)
        {
            return new StreamReader(Source.OpenText());
        }

        public TsObject open_write(TsObject[] args)
        {
            return new FileStream(Source.OpenWrite());
        }

        public TsObject refresh(TsObject[] args)
        {
            Source.Refresh();
            return TsObject.Empty;
        }

        public TsObject replace(TsObject[] args)
        {
            switch (args.Length)
            {
                case 2:
                    return new FileInfo(Source.Replace((string)args[0], (string)args[1]));
                case 3:
                    return new FileInfo(Source.Replace((string)args[0], (string)args[1], (bool)args[2]));
                default:
                    throw new ArgumentException($"Invalid number of arguments passed to {ObjectType}.{nameof(replace)}");
            }
        }

        public static implicit operator TsObject(FileInfo fileInfo) => new TsInstanceWrapper(fileInfo);
        public static explicit operator FileInfo(TsObject obj) => (FileInfo)obj.WeakValue;
    }
}
