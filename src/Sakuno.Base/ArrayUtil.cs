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
    }
}
