using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sakuno.Collections
{
    public class ConcurrentBitArray
    {
        int[] _segments;

        public int Count { get; }

        public bool this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return DangerousGet(index);
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                DangerousSet(index, value);
            }
        }

        public ConcurrentBitArray(int count) : this(count, false) { }
        public ConcurrentBitArray(int count, bool fillValue)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;

            var segmentCount = (count + 31) / 32;
            _segments = new int[segmentCount];

            if (fillValue)
                for (var i = 0; i < segmentCount; i++)
                    _segments[i] = int.MinValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int AsInt32(uint value) => *(int*)&value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe uint AsUInt32(int value) => *(uint*)&value;

        public bool DangerousGet(int index)
        {
            var segmentIndex = index >> 5;
            var segmentOffset = index & 31;
            var mask = 1u << segmentOffset;

            var segment = AsUInt32(Volatile.Read(ref _segments[segmentIndex]));

            return (segment & mask) > 0;
        }
        public void DangerousSet(int index, bool value)
        {
            var segmentIndex = index >> 5;
            var segmentOffset = index & 31;
            var mask = 1u << segmentOffset;
            var shifed = value ? mask : 0;

            uint oldSegment, newSegment;

            do
            {
                oldSegment = AsUInt32(Volatile.Read(ref _segments[segmentIndex]));
                newSegment = (oldSegment & ~mask) | shifed;
            }
            while (oldSegment != newSegment &&
                AsUInt32(Interlocked.CompareExchange(ref _segments[segmentIndex], AsInt32(newSegment), AsInt32(oldSegment))) != oldSegment);
        }
    }
}
