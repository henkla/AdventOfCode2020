using AdventOfCode2020.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain
{
    public class Result
    {
        public class PartialResult
        {
            public int Part { get; private set; }
            public long Result { get; private set; }
            public TimeSpan Elapsed { get; private set; }

            public PartialResult(int part, long result, TimeSpan elapsed)
            {
                Part = part;
                Result = result;
                Elapsed = elapsed;
            }
        }

        public ICollection<PartialResult> Results { get; private set; }
        public string Name { get; private set; }
        public int Day { get; private set; }

        public Result(string name, int day)
        {
            Results = new List<PartialResult>();
            Name = name;
            Day = day;
        }

        public void Print()
        {
            foreach (var result in Results.OrderBy(r => r.Part))
            {
                Console.WriteLine($"- result, part {result.Part}: {result.Result} ({result.Elapsed.Seconds}s {result.Elapsed.Milliseconds}ms)");
            }
        }

        internal void Add(PartialResult partialResult)
        {
            Results.Add(partialResult);
        }
    }
}
