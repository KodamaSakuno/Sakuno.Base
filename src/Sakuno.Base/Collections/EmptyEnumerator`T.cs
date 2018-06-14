using System.Collections;
using System.Collections.Generic;

namespace Sakuno.Collections
{
    public sealed class EmptyEnumerator<T> : IEnumerator<T>
    {
        public static EmptyEnumerator<T> Instance { get; } = new EmptyEnumerator<T>();

        public T Current => default;
        object IEnumerator.Current => Current;

        public bool MoveNext() => false;
        public void Reset() { }
        public void Dispose() { }
    }
}
