using System;

namespace AdventOfCode2020.Domain
{
    public class Result
    {
        public long First { get; set; }
        public long Second { get; set; }

        private string _nameOfChallenge;
        private int _day;

        public Result(string nameOfChallenge, int day)
        {
            First = 0;
            Second = 0;
            _nameOfChallenge = nameOfChallenge;
            _day = day;
        }

        public long GetPart(uint part)
        {
            if (part == 1) return First;
            if (part == 2) return Second;
            
            throw new ArgumentOutOfRangeException($"Part {part} is not a valid part. Check your input parameters.");
        }

        public void Print()
        {
            Console.WriteLine($"{_nameOfChallenge}/{_day.ToString("D2")}:");
            if (First > 0)
                Console.WriteLine($"- part {1}, result is {First}.");
            if (Second > 0)
                Console.WriteLine($"- part {2}, result is {Second}.");

            Console.ReadKey();
        }
    }
}
