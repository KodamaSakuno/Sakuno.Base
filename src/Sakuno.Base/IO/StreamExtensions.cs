using System.IO;

namespace Sakuno.IO
{
    public static class StreamExtensions
    {
        public static int FillBuffer(this Stream stream, byte[] buffer, int offset, int count)
        {
            var remaining = count;

            do
            {
                var rLength = stream.Read(buffer, offset, remaining);
                if (rLength == 0)
                    break;

                remaining -= rLength;
                offset += rLength;
            } while (remaining > 0);

            return count - remaining;
        }
    }
}
