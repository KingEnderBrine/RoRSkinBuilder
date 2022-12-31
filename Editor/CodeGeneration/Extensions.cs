using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RoRSkinBuilder.CodeGeneration
{
    public static class Extensions
    {
        private static Regex jsonEscapeRegex = new Regex("([\"\\\\])", RegexOptions.Compiled);

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return GetOrAdd(dict, key, () => defaultValue);
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> defaultValueFunc)
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }

            return dict[key] = defaultValueFunc();
        }

        public static string EscapeJsonString(this string str)
        {
            return jsonEscapeRegex.Replace(str, "\\$1");
        }
    }
}
