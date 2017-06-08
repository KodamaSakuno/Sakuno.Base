using System;
using System.Threading;

namespace Sakuno
{
    public static class Disposable
    {
        public static IDisposable Create(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new AnonymousDisposable(action);
        }

        public static IDisposable Empty() => EmptyDisposable.Instance;

        sealed class AnonymousDisposable : DisposableObject
        {
            Action _action;

            public AnonymousDisposable(Action action)
            {
                _action = action;
            }

            protected override void DisposeManagedResources() =>
                Interlocked.Exchange(ref _action, null)?.Invoke();
        }

        sealed class EmptyDisposable : IDisposable
        {
            public static readonly EmptyDisposable Instance = new EmptyDisposable();

            public void Dispose() { }
        }
    }
}
