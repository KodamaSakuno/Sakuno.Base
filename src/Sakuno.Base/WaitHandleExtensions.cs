using Sakuno.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sakuno
{
    public static class WaitHandleExtensions
    {
        public static Task WaitOneAsync(this WaitHandle waitHandle)
        {
            if (waitHandle == null)
                throw new ArgumentNullException(nameof(waitHandle));

            var tcs = new TaskCompletionSource();
            var rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle, delegate { tcs.TrySetResult(); }, null, -1, true);

            tcs.Task.ContinueWith(delegate { rwh.Unregister(null); });

            return tcs.Task;
        }
    }
}
