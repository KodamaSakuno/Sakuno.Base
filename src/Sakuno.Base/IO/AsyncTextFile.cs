using System.IO;
using System.Text;

namespace Sakuno.IO
{
    public static class AsyncTextFile
    {
        public static StreamReader Open(string path, Encoding encoding)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, AsyncFile.BufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
            return new StreamReader(fileStream, encoding);
        }

        public static StreamWriter Create(string path, Encoding encoding)
        {
            var fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, AsyncFile.BufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
            return new StreamWriter(fileStream, encoding);
        }
        public static StreamWriter Append(string path, Encoding encoding)
        {
            var fileStream = new FileStream(path, FileMode.Append, FileAccess.ReadWrite, FileShare.Read, AsyncFile.BufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
            return new StreamWriter(fileStream, encoding);
        }
    }
}
