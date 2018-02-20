using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript
{
    public static class FileHandling
    {
        public static void DirectoryDestroy(string dName)
        {
            Directory.Delete(dName, true);
        }

        #region Binary

        private readonly static List<FileStream> _binFiles = new List<FileStream>();
        private readonly static Queue<int> _binSlots = new Queue<int>();

        public static void FileBinClose(int handle)
        {
            var bin = GetBin(handle);
            bin.Flush();
            bin.Dispose();
            _binFiles[handle] = null;
            _binSlots.Enqueue(handle);
        }

        public static int FileBinOpen(string fname, int mode)
        {
            var access = mode == 0 ? FileAccess.Read : (mode == 1 ? FileAccess.Write : FileAccess.ReadWrite);
            var fileStream = new FileStream(fname, FileMode.OpenOrCreate, access);
            int index;
            if (_binSlots.Count == 0)
            {
                index = _binFiles.Count;
                _binFiles.Add(fileStream);
            }
            else
            {
                index = _binSlots.Dequeue();
                _binFiles[index] = fileStream;
            }
            return index;
        }

        public static float FileBinPosition(int handle)
        {
            return GetBin(handle).Position;
        }

        public static float FileBinReadByte(int handle)
        {
            return GetBin(handle).ReadByte();
        }

        public static void FileBinRewrite(int handle)
        {
            var bin = GetBin(handle);
            bin.SetLength(0);
        }

        public static void FileBinSeek(int handle, float pos)
        {
            GetBin(handle).Seek((long)pos, SeekOrigin.Begin);
        } 

        public static float FileBinSize(int handle)
        {
            return GetBin(handle).Length;
        }

        public static float FileBinWriteByte(int handle, int value)
        {
            byte write = unchecked((byte)value);
            var bin = GetBin(handle);

            bin.WriteByte(write);
            return bin.Position;
        }

        public static FileStream GetBin(int index)
        {
            if (index < 0 || index >= _binFiles.Count || _binFiles[index] == null)
                throw new ArgumentOutOfRangeException("index");
            return _binFiles[index];
        }

        #endregion

        #region General

        private static IEnumerator<string> _files;

        public static void FileCopy(string file, string newFile)
        {
            File.Copy(file, newFile, true);
        }

        public static void FileFindClose()
        {
            _files = null;
        }

        public static string FileFindFirst(string mask, int attribs)
        {
            var path = Path.GetDirectoryName(mask);
            var pattern = Path.GetFileName(mask);
            if (attribs == 0)
                _files = Directory.EnumerateFiles(path, pattern).GetEnumerator();
            else
                _files = Directory.EnumerateFiles(path, pattern).Where(f => File.GetAttributes(f).HasFlag((FileAttributes)attribs)).GetEnumerator();

            return FileFindNext();
        }

        public static string FileFindNext()
        {
            if (_files.MoveNext())
                return _files.Current;
            else
                return "";
        }

        #endregion

        #region Text

        // Todo: Finish Text File

        /*private readonly static List<Stream> _textFiles = new List<Stream>();
        private readonly static Dictionary<int, IDisposable> _textHandlers = new Dictionary<int, IDisposable>();
        private readonly static Queue<int> _textSlots = new Queue<int>();

        public static void FileTextClose(int handle)
        {
            var file = GetTextStream(handle);

            var handler = _textHandlers[handle];
            if (handler is StreamWriter writer)
                writer.Flush();

            handler.Dispose();
            file.Dispose();

            _textFiles[handle] = null;
            _textHandlers.Remove(handle);
        }

        public static bool FileTextEof(int handle)
        {
            var file = GetTextReader(handle);
            return file.EndOfStream;
        }

        public static bool FileTextEol(int handle)
        {
            var file = GetTextReader(handle);
            var c = file.Peek();
            return c == '\n' || c == '\r';
        }

        public static int FileTextOpen(string fname, int mode)
        {
            var access = mode == 0 ? FileAccess.Read : (mode == 1 ? FileAccess.Write : FileAccess.ReadWrite);
            var fileStream = new FileStream(fname, FileMode.OpenOrCreate, access);
            int index;
            if (_binSlots.Count == 0)
            {
                index = _binFiles.Count;
                _binFiles.Add(fileStream);
            }
            else
            {
                index = _binSlots.Dequeue();
                _binFiles[index] = fileStream;
            }
            return index;
        }

        public static Stream GetTextStream(int fileId)
        {
            if (fileId < 0 || fileId >= _textFiles.Count)
                throw new ArgumentOutOfRangeException("fileId");

            var result = _textFiles[fileId];

            if (result == null)
                throw new ArgumentException($"No file with the id {fileId} exists.", "fileId");

            return result;
        }

        public static StreamReader GetTextReader(int fileId)
        {
            if (!_textHandlers.TryGetValue(fileId, out var handler))
                throw new ArgumentOutOfRangeException("fileId");
            if (handler is StreamReader reader)
                return reader;

            throw new ArgumentException("The given file id was only open for writing.", "fileId");
        }

        public static StreamWriter GetTextWriter(int fileId)
        {
            if (!_textHandlers.TryGetValue(fileId, out var handler))
                throw new ArgumentOutOfRangeException("fileId");
            if (handler is StreamWriter writer)
                return writer;

            throw new ArgumentException("The given file id was only open for writing.", "fileId");
        }*/

        #endregion
    }
}
