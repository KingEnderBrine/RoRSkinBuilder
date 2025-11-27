using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RoRSkinBuilder
{
    public static class T4Helpers
    {
        public static string ToUpperCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str.Trim(), "\\b[a-z]", (m) => m.Value.ToUpper()), "[^\\w\\d]", "").Replace(" ", "");
        }

        public static string StripSpaces(this string str)
        {
            return str.Replace(" ", "");
        }

        public static string ToLiteral(this bool value)
        {
            return value ? "true" : "false";
        }

        public static string Escape(this string str)
        {
            return str.Replace(@"\", @"\\").Replace("\"", "\\\"");
        }

        public static string ToLiteral(this float value)
        {
            return $"{value.ToString(CultureInfo.InvariantCulture)}f";
        }

        public static string ToNewString(this Color color)
        {
            return $"new Color({color.r.ToLiteral()}, {color.g.ToLiteral()}, {color.b.ToLiteral()}, {color.a.ToLiteral()})";
        }

        public static string ToNewString(this Vector3 vector)
        {
            return $"new Vector3({vector.x.ToLiteral()}, {vector.y.ToLiteral()}, {vector.z.ToLiteral()})";
        }
    }
}
