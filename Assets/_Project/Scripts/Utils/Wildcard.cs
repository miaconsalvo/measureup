using System.Text.RegularExpressions;
using UnityEngine;

namespace Mystie.Utils
{
    // from
    // https://www.codeproject.com/Articles/11556/Converting-Wildcards-to-Regexes
    // by Rei Miyasaka
    class Wildcard : Regex
    {
        public Wildcard(string pattern) : base(WildcardToRegex(pattern)) { }

        public Wildcard(string pattern, RegexOptions options) : base(WildcardToRegex(pattern), options) { }

        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }
    }
}
