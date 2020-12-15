using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode2020.Domain.Day13
{
    public class ShuttleSearch : BaseChallenge
    {
        private string[] _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("example.txt").ToArray();
        }

        protected override void SolveFirst()
        {
            var timestamp = int.Parse(_input[0]);
            var ids = _input[1].Split(',');
            var table = new Dictionary<int, int>();

            foreach (var id in ids.Where(s => !string.Equals(s, "x")).Select(s => int.Parse(s)).ToList())
            {
                var value = 0;
                while (value < timestamp)
                {
                    value += id;
                }

                table.Add(id, value);
            }

            var target = table.Where(kv => kv.Value > timestamp).OrderBy(kv => kv.Value).First();
            Result.First = (target.Value - timestamp) * target.Key;
        }

        protected override void SolveSecond()
        {
            static long GCD(long a, long b)
            {
                while (b != 0) b = a % (a = b); return a;
            }

            var ids = _input[1].Split(',').ToArray();
            var earliestTime = long.Parse(ids[0]);
            var increment = earliestTime;
            foreach (var i in Enumerable.Range(1, ids.Length - 1))
            {
                if (ids[i] == "x") continue;
                var curTime = long.Parse(ids[i]);
                var modValue = curTime - (i % curTime);
                while (earliestTime % curTime != modValue)
                    earliestTime += increment;
                increment = (increment * curTime) / GCD(increment, curTime);
            }

            Result.Second = earliestTime;
        }
    }
}
