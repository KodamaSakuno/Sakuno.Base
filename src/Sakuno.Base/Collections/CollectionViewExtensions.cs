namespace Sakuno.Collections
{
    static class CollectionViewExtensions
    {
        public static int EnsurePositiveIndex(this int index)
        {
            if (index < 0)
                index = ~index;

            return index;
        }
    }
}
