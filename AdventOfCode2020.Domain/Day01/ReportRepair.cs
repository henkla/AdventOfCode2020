using AdventOfCode2020.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day01
{
    public class ReportRepair : BaseChallenge
    {
        private IEnumerable<int> _input;
        private int _target;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt").Select(s => int.Parse(s));
            _target = 2020;
        }

        protected override PartialResult SolveFirst()
        {
            foreach (var number in _input)
            {
                var desiredPair = _target - number;
                if (_input.Contains(desiredPair))
                {
                    return new PartialResult(1, number * desiredPair, Stopwatch.Elapsed);
                }
            }

            throw new ApplicationException("There are no numbers to match given criteria!");
        }

        protected override PartialResult SolveSecond()
        {
            foreach (var num1 in _input)
            {
                foreach (var num2 in _input)
                {
                    var desiredPair = _target - num1 - num2;
                    if (_input.Contains(desiredPair))
                    {
                        return new PartialResult(2, num1 * num2 * desiredPair, Stopwatch.Elapsed);
                    }
                }
            }

            throw new ApplicationException("There are no numbers to match given criteria!");
        }
    }
}
