using System;

namespace Sakuno.Collections
{
    public static class ConcurrencyLevelExtensions
    {
        public static int ToValue(this ConcurrencyLevel level)
        {
            switch (level)
            {
                case ConcurrencyLevel.Default:
                case ConcurrencyLevel.Low:
                    return Math.Max(2, Environment.ProcessorCount / 4);

                case ConcurrencyLevel.Medium:
                    return Math.Max(4, Environment.ProcessorCount / 2);

                case ConcurrencyLevel.High:
                    return Math.Max(8, Environment.ProcessorCount);

                default: throw new ArgumentException(nameof(level));
            }
        }
    }
}
