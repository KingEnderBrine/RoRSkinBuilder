using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RoRSkinBuilder
{
    public static class T4Helpers
    {
        public static string ToUpperCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return Regex.Replace(Regex.Replace(str.Trim(), "\\b[a-z]", (m) => m.Value.ToUpperInvariant()), "[^\\w\\d]", "").Replace(" ", "");
        }

        public static string ToLowerCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (str.Length == 1)
            {
                return str.ToLowerInvariant();
            }
            var newStr = str.ToUpperCamelCase();
            return char.ToLowerInvariant(newStr[0]) + newStr.Substring(1);
        }

        public static string UnityPath(this string path)
        {
            return path?.Replace("\\", "/");
        }

        public static string StripSpaces(this string str)
        {
            return str.Replace(" ", "");
        }

        public static string ToLiteral(this bool value)
        {
            return value ? "true" : "false";
        }

        public static string ToFloatString(this float value)
        {
            return value.ToString("0.##F", CultureInfo.InvariantCulture);
        }

        public static IEnumerable<(T, int)> GetIndexedEnumerable<T>(this IEnumerable<T> enumerable)
        {
            return new IndexedIEnumerable<T>(enumerable);
        }

        private readonly struct IndexedIEnumerable<T> : IEnumerable<(T, int)>
        {
            private readonly IEnumerable<T> baseEnumerable;
            
            public IndexedIEnumerable(IEnumerable<T> enumerable)
            {
                baseEnumerable = enumerable;
            }

            public IEnumerator<(T, int)> GetEnumerator()
            {
                return new IndexedEnumerator<T>(baseEnumerable.GetEnumerator());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new IndexedEnumerator<T>(baseEnumerable.GetEnumerator());
            }
        }

        private struct IndexedEnumerator<T> : IEnumerator<(T, int)>
        {
            private readonly IEnumerator<T> baseEnumerator;
            private int index;

            public IndexedEnumerator(IEnumerator<T> enumerator)
            {
                baseEnumerator = enumerator;
                index = -1;
            }

            public (T, int) Current => (baseEnumerator.Current, index);

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                baseEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (baseEnumerator.MoveNext())
                {
                    index++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                baseEnumerator.Reset();
                index = -1;
            }
        }
    }
}
