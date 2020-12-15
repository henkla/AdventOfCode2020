using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day09
{
    public class EncodingError : BaseChallenge
    {
        private IEnumerable<long> _input;
        private int _sizeOfPreamble;
        private long _targetValue;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt").Select(long.Parse);
            _sizeOfPreamble = 25;
        }

        protected override PartialResult SolveFirst()
        {
            long sum = 0;
            for (int index = _sizeOfPreamble; index < _input.Count(); index++)
            {
                var preamble = _input.Skip(index - _sizeOfPreamble).Take(_sizeOfPreamble);
                sum = _input.ElementAt(index);

                if (!preamble.Where(term => preamble.Contains(sum - term) && (sum - term) != term).Any())
                    break;
            }

            _targetValue = sum;
            return new PartialResult(1, sum, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            if (_targetValue == default)
            {
                SolveFirst();
                Stopwatch.Restart();
            }

            long sum = 0;
            
            // we need to investigate where the contiguous range of terms begins, starting
            // at zero (all the way up to possibly the index where the target value resides)
            for (int startingIndex = 0; startingIndex < Array.FindIndex(_input.ToArray(), number => number == _targetValue); startingIndex++)
            {
                // this is the sum that we want to equal the target value 
                // later on
                var possibleContiguousSum = _input.ElementAt(startingIndex);

                // add up all following numbers from the list into the sum up until we
                // either A) receive a sum that as larger than the target (in which case, 
                // the contiguous range was not the correct one) or B) we find a sum that
                // is equal to the target value (in which case, we have the range that we want)
                var sequencialIndex = startingIndex;
                while (possibleContiguousSum < _targetValue)
                {
                    // get the element that corresponds with the index
                    var nextTerm = _input.ElementAt(++sequencialIndex);

                    // add this element to the sum
                    possibleContiguousSum += nextTerm;
                }

                // check if the sum is equal to the value that we are looking for
                if (possibleContiguousSum == _targetValue)
                {
                    // the result was supposed to be the smallest term and the
                    // largest term added together, so make sure to get the range
                    var targetRange = _input.ToList().GetRange(startingIndex, sequencialIndex - startingIndex + 1);
                    sum = targetRange.Min() + targetRange.Max();
                    break;
                }
            }

            return new PartialResult(2, sum, Stopwatch.Elapsed);
        }
    }
}
