using System.Text.RegularExpressions;

namespace RoRSkinBuilder
{
    public static class T4Helpers
    {
        public static string ToUpperCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str.Trim(), "\\b[a-z]", (m) => m.Value.ToUpper()), "[^\\w\\d]", "").Replace(" ", "");
        }
    }
}
