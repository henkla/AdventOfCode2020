using System.Collections.Generic;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day17
{
    public class DummyChallenge : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("example.txt");
        }

        protected override PartialResult SolveFirst()
        {
            return new PartialResult(1, 0, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            return new PartialResult(2, 0, Stopwatch.Elapsed);
        }
    }
}
