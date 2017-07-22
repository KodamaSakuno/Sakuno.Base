using System.ComponentModel;
using System.Net.Sockets;

namespace Sakuno.Net
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SocketExtensions
    {
        public static SocketAsyncOperationContext AcceptAsync(this Socket socket, SocketAsyncOperationContext context)
        {
            var awaiter = context.GetAwaiter();

            awaiter.Reset();

            if (!socket.AcceptAsync(context.Argument))
                awaiter.Complete();

            return context;
        }

        public static SocketAsyncOperationContext ConnectAsync(this Socket socket, SocketAsyncOperationContext context)
        {
            var awaiter = context.GetAwaiter();

            awaiter.Reset();

            if (!socket.ConnectAsync(context.Argument))
                awaiter.Complete();

            return context;
        }

        public static SocketAsyncOperationContext SendAsync(this Socket socket, SocketAsyncOperationContext context)
        {
            var awaiter = context.GetAwaiter();

            awaiter.Reset();

            if (!socket.SendAsync(context.Argument))
                awaiter.Complete();

            return context;
        }

        public static SocketAsyncOperationContext ReceiveAsync(this Socket socket, SocketAsyncOperationContext context)
        {
            var awaiter = context.GetAwaiter();

            awaiter.Reset();

            if (!socket.ReceiveAsync(context.Argument))
                awaiter.Complete();

            return context;
        }

        public static SocketAsyncOperationContext SendToAsync(this Socket socket, SocketAsyncOperationContext context)
        {
            var awaiter = context.GetAwaiter();

            awaiter.Reset();

            if (!socket.SendToAsync(context.Argument))
                awaiter.Complete();

            return context;
        }

        public static SocketAsyncOperationContext ReceiveFromAsync(this Socket socket, SocketAsyncOperationContext context)
        {
            var awaiter = context.GetAwaiter();

            awaiter.Reset();

            if (!socket.ReceiveFromAsync(context.Argument))
                awaiter.Complete();

            return context;
        }

        public static SocketAsyncOperationContext DisconnectAsync(this Socket socket, SocketAsyncOperationContext context)
        {
            var awaiter = context.GetAwaiter();

            awaiter.Reset();

            if (!socket.DisconnectAsync(context.Argument))
                awaiter.Complete();

            return context;
        }
    }
}
