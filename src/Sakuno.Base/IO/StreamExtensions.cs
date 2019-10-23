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
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (buffer.Length == 0)
                return 0;

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
        public static int FillBuffer(this Stream stream, Span<byte> buffer)
        {
            if (buffer.IsEmpty)
                return 0;

            var originalLength = buffer.Length;

            do
            {
                var length = stream.Read(buffer);
                if (length == 0)
                    break;

                buffer = buffer.Slice(length);
            } while (!buffer.IsEmpty);

            return originalLength - buffer.Length;
        }

        public static async ValueTask<int> FillBufferAsync(this Stream stream, Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (buffer.IsEmpty)
                return 0;

            var originalLength = buffer.Length;

            do
            {
                var length = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
                if (length == 0)
                    break;

                buffer = buffer.Slice(length);
            } while (!buffer.IsEmpty);

            return originalLength - buffer.Length;
        }
#endif
    }
}
