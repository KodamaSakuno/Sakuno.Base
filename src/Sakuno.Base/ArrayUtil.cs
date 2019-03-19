using System;

namespace Sakuno
{
    public static class ArrayUtil
    {
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
