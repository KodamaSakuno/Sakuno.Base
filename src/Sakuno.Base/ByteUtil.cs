using System.Threading.Tasks;

namespace Sakuno
{
    public static class ByteUtil
    {
        static Task<byte>[] _taskResults = new Task<byte>[256];

        public static Task<byte> AsTask(byte b)
        {
            var result = _taskResults[b];

            if (result == null)
            {
                result = Task.FromResult(b);
                _taskResults[b] = result;
            }

            return result;
        }
    }
}
