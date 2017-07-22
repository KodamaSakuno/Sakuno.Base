using System.ComponentModel;
using System.IO;

namespace Sakuno.IO
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StreamExtensions
    {
        public static int FillBuffer(this Stream stream, byte[] buffer, int offset, int count)
        {
            var remaining = count;

            do
            {
                var length = stream.Read(buffer, offset, remaining);
                if (length == 0)
                    break;

                remaining -= length;
                offset += length;
            } while (remaining > 0);

            return count - remaining;
        }
    }
}
