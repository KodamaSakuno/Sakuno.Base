using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sakuno.Threading
{
    public sealed class TaskCompletionSource
    {
        TaskCompletionSource<object?> _tcs;

        public Task Task => _tcs.Task;

        public TaskCompletionSource()
        {
            _tcs = new TaskCompletionSource<object?>();
        }
        public TaskCompletionSource(object state)
        {
            _tcs = new TaskCompletionSource<object?>(state);
        }
        public TaskCompletionSource(TaskCreationOptions creationOptions)
        {
            _tcs = new TaskCompletionSource<object?>(creationOptions);
        }
        public TaskCompletionSource(object state, TaskCreationOptions creationOptions)
        {
            _tcs = new TaskCompletionSource<object?>(state, creationOptions);
        }

        public void SetResult() => _tcs.SetResult(null);
        public bool TrySetResult() => _tcs.TrySetResult(null);

        public void SetCanceled() => _tcs.SetCanceled();
        public bool TrySetCanceled() => _tcs.TrySetCanceled();

        public void SetException(Exception exception) => _tcs.SetException(exception);
        public void SetException(IEnumerable<Exception> exceptions) => _tcs.SetException(exceptions);
        public bool TrySetException(Exception exception) => _tcs.TrySetException(exception);
        public bool TrySetException(IEnumerable<Exception> exceptions) => _tcs.TrySetException(exceptions);
    }
}
