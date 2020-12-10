using System.Collections.Generic;

namespace AdventOfCode2020.Domain.Day11
{
    public class Dummy : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("example");
        }

        protected override void SolveFirst()
        {
            throw new System.NotImplementedException();
        }

        protected override void SolveSecond()
        {
            throw new System.NotImplementedException();
        }
    }
}
