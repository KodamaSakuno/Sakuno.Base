namespace Sakuno
{
    public static class BoxedConstants
    {
        public static class Boolean
        {
            public static readonly object True = true;
            public static readonly object False = false;
        }

        public static class Int32
        {
            public static readonly object Zero = 0;
            public static readonly object NegativeOne = -1;
        }

        public static class Int64
        {
            public static readonly object Zero = 0L;
            public static readonly object NegativeOne = -1L;
        }

        public static class Double
        {
            public static readonly object Zero = 0.0;
            public static readonly object One = 1.0;

            public static readonly object NaN = double.NaN;
        }

        public static class Structure<T> where T : struct
        {
            public static readonly object Default = default(T);
        }
    }
}
