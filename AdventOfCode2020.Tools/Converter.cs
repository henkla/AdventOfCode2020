using System;

namespace AdventOfCode2020.Tools
{
    public class Converter
    {
        public long Base2ToBase10(string binary)
        {
            return Convert.ToInt64(binary, 2);
        }

        public string Base10ToBase2(int source, int length)
        {
            return Convert.ToString(source, 2).PadLeft(length, '0');
        }
    }
}
