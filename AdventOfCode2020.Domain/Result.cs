using System;

namespace AdventOfCode2020.Domain
{
    public class Result
    {
        public long First { get; set; }
        public long Second { get; set; }

        public Result()
        {
            First = 0;
            Second = 0;
        }

        public long GetPart(uint part)
        {
            if (part == 1) return First;
            if (part == 2) return Second;
            
            throw new ArgumentOutOfRangeException($"Part {part} is not a valid part. Check your input parameters.");
        }
    }
}
