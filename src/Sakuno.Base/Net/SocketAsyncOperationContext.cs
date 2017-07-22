using System.Net;
using System.Net.Sockets;

namespace Sakuno.Net
{
    public sealed class SocketAsyncOperationContext : DisposableObject
    {
        SocketAsyncEventArgs _argument;

        internal SocketAsyncEventArgs Argument => _argument;

        SocketAsyncOperationAwaiter _awaiter;

        public Socket AcceptSocket
        {
            get => _argument.AcceptSocket;
            set => _argument.AcceptSocket = value;
        }

        public EndPoint RemoteEndPoint
        {
            get => _argument.RemoteEndPoint;
            set => _argument.RemoteEndPoint = value;
        }

        public Socket ConnectSocket => _argument.ConnectSocket;

        public byte[] Buffer => _argument.Buffer;

        public int BytesTransferred => _argument.BytesTransferred;

        public SocketError LastError => _argument.SocketError;

        public SocketAsyncOperationContext()
        {
            _argument = new SocketAsyncEventArgs();
            _awaiter = new SocketAsyncOperationAwaiter(_argument);
        }

        public SocketAsyncOperationAwaiter GetAwaiter() => _awaiter;

        public void SetBuffer(int offset, int count) =>
            _argument.SetBuffer(offset, count);
        public void SetBuffer(byte[] buffer) => SetBuffer(buffer, 0, buffer != null ? buffer.Length : 0);
        public void SetBuffer(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                _argument.SetBuffer(null, 0, 0);
                return;
            }

            _argument.SetBuffer(buffer, offset, count);
        }

        protected override void DisposeManagedResources() => _awaiter.Dispose();
    }
}
