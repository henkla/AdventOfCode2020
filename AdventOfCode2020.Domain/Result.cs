using System;

namespace AdventOfCode2020.Domain
{
    public class Result
    {
        public long First { get; internal set; }
        public long Second { get; internal set; }

        public string Name { get; private set; }
        public int Day { get; private set; }

        public Result(string name, int day)
        {
            Name = name;
            Day = day;
        }

        public void Print()
        {
            Console.WriteLine($"\n{Day:D2} / {Name}");

            if (First != default)
                Console.WriteLine($"- part {1}: {First}");
            if (Second != default)
                Console.WriteLine($"- part {2}: {Second}");

            Console.ReadKey();
        }
    }
}
