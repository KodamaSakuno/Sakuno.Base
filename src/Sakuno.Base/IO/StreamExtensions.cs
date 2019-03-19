using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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

#if NETSTANDARD2_1
        public static async ValueTask<int> FillBuffer(this Stream stream, Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            var remaining = buffer.Length;
            var offset = 0;

            do
            {
                var length = await stream.ReadAsync(buffer, cancellationToken);
                if (length == 0)
                    break;

                remaining -= length;
                offset += length;
            } while (remaining > 0);

            return buffer.Length - remaining;
        }
#endif
    }
}
