using System;
using System.Threading;

namespace Sakuno
{
    public abstract class DisposableObject : IDisposable
    {
        volatile int _isDisposed;
        public bool IsDisposed => _isDisposed != 0;

        ~DisposableObject() => Dispose(false);
        public void Dispose()
        {
            if (_isDisposed != 0 || Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
                return;

            try
            {
                Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
                DisposeManagedResources();

            DisposeNativeResources();
        }

        protected virtual void DisposeManagedResources() { }
        protected virtual void DisposeNativeResources() { }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}
