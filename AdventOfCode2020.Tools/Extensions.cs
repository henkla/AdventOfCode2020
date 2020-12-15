using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Tools
{
    public static class Extensions
    {
        #region string extensions
        
        public static int ExtractNumbers(this string source)
        {
            return int.Parse(Regex.Match(source, @"\d+").Value);
        }

        #endregion

        #region collections extensions

        public static void AddOrUpdate<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        #endregion
    }
}
