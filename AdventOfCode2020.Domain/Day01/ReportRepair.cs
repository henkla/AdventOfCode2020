using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day01
{
    public class ReportRepair : BaseChallenge
    {
        private IEnumerable<int> _input;
        private int _target;

        public ReportRepair()
        {

        }

        protected override void Initialize()
        {
            _input = _inputHelper.GetInputAsLines("input.txt").Select(s => int.Parse(s));
            _target = 2020;
        }

        protected override void SolveFirst()
        {
            foreach (var number in _input)
            {
                var desiredPair = _target - number;
                if (_input.Contains(desiredPair))
                {
                    _result.First = number * desiredPair;
                    return;
                }
            }

            throw new ApplicationException("There are no numbers to match given criteria!");
        }

        protected override void SolveSecond()
        {
            foreach (var num1 in _input)
            {
                foreach (var num2 in _input)
                {
                    var desiredPair = _target - num1 - num2;
                    if (_input.Contains(desiredPair))
                    {
                        _result.Second = num1 * num2 * desiredPair;
                        return;
                    }
                }
            }
        }
    }
}
