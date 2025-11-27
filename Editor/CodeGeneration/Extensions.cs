using System;
using System.Collections.Generic;
using System.Text;

namespace RoRSkinBuilder.CodeGeneration
{
    public static class Extensions
    {
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

        public static int AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            var count = 0;

            foreach (var item in items)
            {
                if (set.Add(item))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
