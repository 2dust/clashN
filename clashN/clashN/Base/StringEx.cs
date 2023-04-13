using System.IO;

namespace ClashN.Base
{
    internal static class StringEx
    {
        public static bool BeginWithAny(this string? s, IEnumerable<char> chars)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            return chars.Contains(s[0]);
        }

        public static IEnumerable<string> NonWhiteSpaceLines(this TextReader reader)
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                yield return line;
            }
        }

        public static string TrimEx(this string? value)
        {
            return value == null ? string.Empty : value.Trim();
        }
    }
}