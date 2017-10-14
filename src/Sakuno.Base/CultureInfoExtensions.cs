using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Sakuno
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CultureInfoExtensions
    {
        public static bool IsAncestorOf(this CultureInfo cultureInfo, CultureInfo value)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            while (value != CultureInfo.InvariantCulture)
            {
                if (value.Equals(cultureInfo))
                    return true;

                value = value.Parent;
            }

            return false;
        }
        public static bool IsDescendantOf(this CultureInfo cultureInfo, CultureInfo value)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            while (cultureInfo != CultureInfo.InvariantCulture)
            {
                if (cultureInfo.Equals(value))
                    return true;

                cultureInfo = cultureInfo.Parent;
            }

            return false;
        }

        public static IEnumerable<CultureInfo> EnumerateAncestors(this CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            return EnumerateAncestorsCore(cultureInfo, false);
        }
        public static IEnumerable<CultureInfo> EnumerateAncestorsAndSelf(this CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            return EnumerateAncestorsCore(cultureInfo, true);
        }
        static IEnumerable<CultureInfo> EnumerateAncestorsCore(CultureInfo cultureInfo, bool withSelf)
        {
            if (cultureInfo == CultureInfo.InvariantCulture)
                yield break;

            if (!withSelf)
                cultureInfo = cultureInfo.Parent;

            for (; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
                yield return cultureInfo;
        }
    }
}
