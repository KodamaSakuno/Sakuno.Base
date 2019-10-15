using System;
using System.Threading;

namespace Sakuno
{
    public static class Disposable
    {
        public static IDisposable Empty { get; } = new EmptyDisposable();

        public static IDisposable Create(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new AnonymousDisposable(action);
        }

        sealed class AnonymousDisposable : DisposableObject
        {
            Action? _action;

            public AnonymousDisposable(Action? action)
            {
                _action = action;
            }

            protected override void DisposeManagedResources() =>
                Interlocked.Exchange(ref _action, null)?.Invoke();
        }

        sealed class EmptyDisposable : IDisposable
        {
            public void Dispose() { }
        }
    }
}
