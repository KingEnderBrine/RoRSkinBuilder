using System;
using System.Globalization;
using UnityEngine;

namespace RoRSkinBuilder.SkinAPI.Configuration
{
    public static class Converter
    {
        public static T Convert<T>(string value) => (T)Convert(value, typeof(T));

        public static object Convert(string value, Type type)
        {
            if (type == typeof(Color))
            {
                return ConvertColor(value);
            }
            if (type == typeof(string))
            {
                return ConvertString(value);
            }
            if (type == typeof(int))
            {
                return ConvertInt(value);
            }
            if (type == typeof(float))
            {
                return ConvertFloat(value);
            }
            if (type == typeof(bool))
            {
                return ConvertBool(value);
            }
            return default;
        }

        private static bool ConvertBool(string value)
        {
            return bool.Parse(value);
        }

        private static float ConvertFloat(string value)
        {
            return float.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        private static int ConvertInt(string value)
        {
            return int.Parse(value);
        }

        private static string ConvertString(string value)
        {
            return value;
        }

        private static Color ConvertColor(string value)
        {
            if (!ColorUtility.TryParseHtmlString("#" + value.Trim('#', ' '), out var c))
                throw new FormatException("Invalid color string, expected hex #RRGGBBAA");
            return c;
        }
    }
}
