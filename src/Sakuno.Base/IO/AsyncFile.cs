using System.IO;

namespace Sakuno.IO
{
    public static class AsyncFile
    {
        public const int BufferSize = 4096;

        public static FileStream Create(string path) =>
            new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, BufferSize, FileOptions.Asynchronous);

        public static FileStream OpenRead(string path) =>
            new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, BufferSize, FileOptions.Asynchronous);
        public static FileStream OpenWrite(string path) =>
            new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, BufferSize, FileOptions.Asynchronous);
    }
}
