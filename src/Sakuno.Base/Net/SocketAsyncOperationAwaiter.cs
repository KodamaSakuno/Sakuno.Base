using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Sakuno.Net
{
    public sealed class SocketAsyncOperationAwaiter : INotifyCompletion
    {
        static readonly Action Sentinel = () => { };

        SocketAsyncEventArgs _argument;

        volatile int _state = 1;
        public bool IsCompleted => _state == 1;

        Action _continuation;

        internal SocketAsyncOperationAwaiter(SocketAsyncEventArgs argument)
        {
            _argument = argument;
            _argument.Completed += EventArgs_Completed;
        }

        internal void Complete() => Interlocked.Exchange(ref _state, 1);

        internal void Reset()
        {
            Interlocked.Exchange(ref _state, 0);
            Interlocked.Exchange(ref _continuation, null);
        }

        public SocketError GetResult() => _argument.SocketError;

        public void OnCompleted(Action continuation)
        {
            if (_continuation != Sentinel && Interlocked.CompareExchange(ref _continuation, continuation, null) != Sentinel)
                return;

            Complete();

            Task.Run(continuation);
        }

        void EventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            var continuation = _continuation ?? Interlocked.CompareExchange(ref _continuation, Sentinel, null);

            Complete();

            continuation?.Invoke();
        }

        internal void Dispose()
        {
            var argument = Interlocked.Exchange(ref _argument, null);
            if (argument == null)
                return;

            argument.Completed -= EventArgs_Completed;
        }
    }
}
