using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace RoRSkinBuilder.SkinAPI.Configuration
{
    public static class BepInExConfigReader
    {
        private static readonly Dictionary<Type, Dictionary<string, FieldInfo>> fieldCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();

        public static Dictionary<string, object> ReadConfigDictionary(string path)
        {
            var instance = new Dictionary<string, object>();
            Type nextType = null;
            foreach (var line in File.ReadLines(path))
            {
                if (line.StartsWith("[") && line.StartsWith("]"))
                {
                    continue;
                }

                if (line.StartsWith("#"))
                {
                    if (line.StartsWith("# Setting type:"))
                    {
                        nextType = GetTypeByName(line.Substring(15).Trim());
                    }
                    continue;
                }
                
                var split = line.Split(new[] { '=' }, 2);

                var key = split[0].Trim();
                string value;
                if (split.Length == 1)
                {
                    value = null;
                }
                else
                {
                    value = split[1].Trim();
                }
                if (nextType == null)
                {
                    continue;
                }

                instance[key] = string.IsNullOrWhiteSpace(value) ? null : Converter.Convert(value, nextType);
            }

            return instance;
        }

        private static Type GetTypeByName(string name)
        {
            switch (name)
            {
                case "Single":
                    return typeof(float);
                case "Color":
                    return typeof(Color);
                case "Boolean":
                    return typeof(bool);
                case "Int32":
                    return typeof(int);
                case "String":
                    return typeof(string);
            }
            return null;
        }

        public static T ReadConfig<T>(string path) where T : class
        {
            var instance = Activator.CreateInstance<T>();
            foreach (var line in File.ReadLines(path))
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line.StartsWith("[") && line.StartsWith("]"))
                {
                    continue;
                }
                var split = line.Split(new[] { '=' }, 2);

                var key = split[0].Trim();
                string value;
                if (split.Length == 1)
                {
                    value = null;
                }
                else
                {
                    value = split[1].Trim();
                }
                if (!fieldCache.TryGetValue(typeof(T), out var fields))
                {
                    fields = new Dictionary<string, FieldInfo>();
                }
                if (!fields.TryGetValue(key, out var fieldInfo))
                {
                    fieldInfo = typeof(T).GetField(key);
                }
                if (fieldInfo == null)
                {
                    continue;
                }

                fieldInfo.SetValue(instance, string.IsNullOrWhiteSpace(value) ? null : Converter.Convert(value, fieldInfo.FieldType));
            }

            return instance;
        }
    }
}
