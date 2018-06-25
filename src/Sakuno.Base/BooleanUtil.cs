using System.Runtime.CompilerServices;

namespace Sakuno
{
    public static class BooleanUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetBoxed(bool value) => value ? BoxedConstants.Boolean.True : BoxedConstants.Boolean.False;
    }
}
