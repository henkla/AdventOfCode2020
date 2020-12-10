using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day10
{
    internal class AdapterArray : BaseChallenge
    {
        private int _baseDiff;
        private List<Adapter> _adapters;

        protected override void Initialize()
        {
            var input = _inputHelper.GetInputAsLines("input.txt").Select(s => int.Parse(s)).OrderBy(i => i);
            _baseDiff = 3;

            _adapters = new List<Adapter>();
            foreach (var line in input)
            {
                _adapters.Add(new Adapter
                {
                    Joltage = line
                });
            }
        }

        protected override void SolveFirst()
        {
            // first, we need a base adapter (the actual outlet)
            var current = new Adapter { Joltage = 0 };

            // connect all available adapters until they're all used up
            while (_adapters.Where(a => !a.IsInUse).Any())
            {
                // make sure we fullfill the requirements
                var next = _adapters.Where(a => a.Joltage - current.Joltage >= 0 && a.Joltage - current.Joltage <= _baseDiff && !a.IsInUse).First();

                // plug in the new one to the last used
                next.PlugInTo(current);

                // the last used is updated
                current = next;
            }

            // we need to keep track of the 1-diffs and 3-diffs
            var ones = 0;
            var threes = 0;

            // begin from the outer adapter all the way to the first used
            while (current.Previous != default)
            {
                if (current.Differance == 1)
                    ones++;
                else if (current.Differance == 3)
                    threes++;

                current = current.Previous;
            }

            // we must not forget the joltage-diff (baseDiff) between the outer adapter and
            // the actual equipment we are connecting to the chain of adapters
            _result.First = _baseDiff + ones * threes;
        }

        protected override void SolveSecond()
        {
            throw new System.NotImplementedException();
        }
    }
}
