using System;
using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class ArrayUtil
    {
        public static string ToHexString(this byte[] bytes)
        {
            var buffer = new char[bytes.Length * 2];
            var position = 0;

            foreach (var b in bytes)
            {
                buffer[position++] = GetHexValue(b / 16);
                buffer[position++] = GetHexValue(b % 16);
            }

            return new string(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char GetHexValue(int value) => (char)(value < 10 ? value + '0' : value - 10 + 'a');

        public static bool Equals(byte[] x, byte[] y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));

            if (x.Length != y.Length)
                return false;

            return Equals(x, y, x.Length);
        }
        static unsafe bool Equals(byte[] x, byte[] y, int length)
        {
            if (x == y)
                return true;

            var remaining = length;

            fixed (byte* ptrx = x)
            fixed (byte* ptry = y)
            {
                var px = ptrx;
                var py = ptry;

                while (remaining >= 8)
                {
                    if (*(long*)px != *(long*)py)
                        return false;

                    px += 8;
                    py += 8;
                    remaining -= 8;
                }

                while (remaining > 0)
                {
                    if (*px != *py)
                        return false;

                    px++;
                    py++;
                    remaining--;
                }
            }

            return true;
        }
    }
}
