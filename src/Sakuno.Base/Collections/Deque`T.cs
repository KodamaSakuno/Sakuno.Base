using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sakuno.Collections
{
    public class Deque<T> : IEnumerable<T>, ICollection, IReadOnlyCollection<T>
    {
        T[] _array;

        int _count;
        public int Count => _count;

        int _head, _tail;

        int _version;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException();

                return GetElement(index);
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException();

                _array[GetOffset(index)] = value;
                _version++;
            }
        }

        bool ICollection.IsSynchronized => false;

        object _threadSyncLock;
        object ICollection.SyncRoot =>
            _threadSyncLock ?? Interlocked.CompareExchange(ref _threadSyncLock, new object(), null);

        public Deque() =>_array = Array.Empty<T>();
        public Deque(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _array = new T[capacity];
        }
        public Deque(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            _array = new T[4];

            foreach (var item in collection)
                Enqueue(item);
        }

        public void Enqueue(T item)
        {
            GrowCapacityIfNecessary();

            _array[_tail] = item;

            _tail = (_tail + 1) % _array.Length;
            _count++;
            _version++;
        }
        public void EnqueueFront(T item)
        {
            GrowCapacityIfNecessary();

            _head = (_head + _array.Length - 1) % _array.Length;
            _array[_head] = item;

            _count++;
            _version++;
        }
        public void Enqueue(T item, QueueSide side)
        {
            switch (side)
            {
                case QueueSide.Back:
                    Enqueue(item);
                    break;

                case QueueSide.Front:
                    EnqueueFront(item);
                    break;

                default: throw new ArgumentException(nameof(side));
            }
        }

        public T Dequeue()
        {
            if (_count == 0)
                throw new InvalidOperationException();

            var result = _array[_head];
            _array[_head] = default;

            _head = (_head + 1) % _array.Length;
            _count--;
            _version++;

            return result;
        }
        public T DequeueBack()
        {
            if (_count == 0)
                throw new InvalidOperationException();

            _tail = (_tail + _array.Length - 1) % _array.Length;

            var result = _array[_tail];
            _array[_tail] = default;

            _count--;
            _version++;

            return result;
        }
        public T Dequeue(QueueSide side)
        {
            switch (side)
            {
                case QueueSide.Back:
                    return DequeueBack();

                case QueueSide.Front:
                    return Dequeue();

                default: throw new ArgumentException(nameof(side));
            }
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException();

            return _array[_head];
        }
        public T PeekBack()
        {
            if (_count == 0)
                throw new InvalidOperationException();

            var tail = (_tail + _array.Length - 1) % _array.Length;

            return _array[tail];
        }
        public T Peek(QueueSide side)
        {
            switch (side)
            {
                case QueueSide.Back:
                    return PeekBack();

                case QueueSide.Front:
                    return Peek();

                default: throw new ArgumentException(nameof(side));
            }
        }

        public void Clear()
        {
            if (_head < _tail)
                Array.Clear(_array, _head, _count);
            else
            {
                Array.Clear(_array, _head, _array.Length - _head);
                Array.Clear(_array, 0, _tail);
            }

            _head = _tail = 0;
            _count = 0;
            _version++;
        }

        public bool Contains(T item)
        {
            var index = _head;
            var count = _count;

            var comparer = EqualityComparer<T>.Default;

            while (count-- > 0)
            {
                if (item == null && _array[index] == null)
                    return true;
                else if (_array[index] != null && comparer.Equals(_array[index], item))
                    return true;

                index = (index + 1) % _array.Length;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetOffset(int index) => (_head + index) % _array.Length;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T GetElement(int index) => _array[GetOffset(index)];

        void GrowCapacityIfNecessary()
        {
            if (_count != _array.Length)
                return;

            var capacity = Math.Max(_array.Length * 2, _array.Length + 4);
            var array = new T[capacity];

            if (_count > 0)
            {
                if (_head < _tail)
                    Array.Copy(_array, _head, array, 0, _count);
                else
                {
                    Array.Copy(_array, _head, array, 0, _array.Length - _head);
                    Array.Copy(_array, 0, array, _array.Length - _head, _tail);
                }
            }

            _array = array;

            _head = 0;
            _tail = _count != capacity ? _count : 0;
            _version++;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);
        public ReverseEnumerator GetReverseEnumerator() => new ReverseEnumerator(this);
        public IEnumerator<T> GetEnumerator(QueueSide side)
        {
            switch (side)
            {
                case QueueSide.Back:
                    return GetReverseEnumerator();

                case QueueSide.Front:
                    return GetEnumerator();

                default: throw new ArgumentException(nameof(side));
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException();

            if (array.Rank != 1 || array.GetLowerBound(0)!=0)
                throw new RankException();

            var length = array.Length;

            if (index < 0 || index > length)
                throw new ArgumentOutOfRangeException();

            if (length - index < _count)
                throw new ArgumentException();

            var totalCount = _count;
            if (totalCount == 0)
                return;

            var count = Math.Min(_array.Length - _head, totalCount);

            Array.Copy(_array, _head, array, index, count);
            totalCount -= count;

            if (count > 0)
                Array.Copy(_array, 0, array, index + _array.Length - _head, count);
        }

        public struct Enumerator : IEnumerator<T>
        {
            Deque<T> _owner;

            int _version;

            int _index;

            T _current;
            public T Current
            {
                get
                {
                    if (_index < 0)
                        throw new InvalidOperationException();

                    return _current;
                }
            }
            object IEnumerator.Current => Current;

            public Enumerator(Deque<T> owner)
            {
                _owner = owner;

                _version = _owner._version;

                _index = -1;
                _current = default;
            }

            public bool MoveNext()
            {
                if (_version != _owner._version)
                    throw new InvalidOperationException();

                if (_index == -2)
                    return false;

                _index++;

                if (_index == _owner._count)
                {
                    Dispose();

                    return false;
                }

                _current = _owner.GetElement(_index);

                return true;
            }

            public void Dispose()
            {
                _index = -2;
                _current = default;
            }
            void IEnumerator.Reset()
            {
                if (_version != _owner._version)
                    throw new InvalidOperationException();

                _index = -1;
                _current = default;
            }
        }
        public struct ReverseEnumerator : IEnumerator<T>
        {
            Deque<T> _owner;

            int _version;

            int _index;

            T _current;
            public T Current
            {
                get
                {
                    if (_index < 0)
                        throw new InvalidOperationException();

                    return _current;
                }
            }
            object IEnumerator.Current => Current;

            public int Version { get => _version; set => _version = value; }

            public ReverseEnumerator(Deque<T> owner)
            {
                _owner = owner;

                _version = _owner._version;

                _index = _owner._count;
                _current = default;
            }

            public bool MoveNext()
            {
                if (_version != _owner._version)
                    throw new InvalidOperationException();

                if (_index == -2)
                    return false;

                _index--;

                if (_index == -1)
                {
                    Dispose();

                    return false;
                }

                _current = _owner.GetElement(_index);

                return true;
            }

            public void Dispose()
            {
                _index = -2;
                _current = default;
            }
            void IEnumerator.Reset()
            {
                if (_version != _owner._version)
                    throw new InvalidOperationException();

                _index = -1;
                _current = default;
            }
        }
    }
}
