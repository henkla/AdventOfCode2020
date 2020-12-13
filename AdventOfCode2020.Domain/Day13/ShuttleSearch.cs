using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day13
{
    public class ShuttleSearch : BaseChallenge
    {
        private string[] _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt").ToArray();
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
            throw new System.NotImplementedException();
        }
    }
}
