using System.Collections.Generic;

namespace AdventOfCode2020.Domain.Day12
{
    public class DummyChallenge : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("example.txt");
        }

        protected override void SolveFirst()
        {
        }

        protected override void SolveSecond()
        {
        }
    }
}
