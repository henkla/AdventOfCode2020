using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day10
{
    public class AdapterArray : BaseChallenge
    {
        private IEnumerable<int> _input;
        private int _maxDiff;

        protected override void Initialize()
        {
            _maxDiff = 3;
            _input = InputReader.ReadFile("input.txt")
                .Select(s => int.Parse(s))
                .OrderBy(i => i)
                .ToList();
        }

        protected override PartialResult SolveFirst()
        {
            var joltages = _input;
            var ones = 0;
            var threes = 1;

            var currentJolt = 0;
            foreach (var nextJolt in joltages)
            {
                if (nextJolt - currentJolt == 3) 
                    threes++;
                if (nextJolt - currentJolt == 1) 
                    ones++;

                currentJolt = nextJolt;
            }

            return new PartialResult(1, ones * threes, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            var joltages = _input.Append(0).OrderBy(i => i).ToArray();
            var permutations = new long[joltages.Length];
            
            // there's always one permutation, so start out 
            // with the first one
            permutations[0] = 1;

            // for each joltage (starting with the second one) ->
            foreach (var i in Enumerable.Range(1, joltages.Length - 1))
            {
                // -> investigate previous joltages -> 
                foreach (var j in Enumerable.Range(0, i))
                {
                    // -> and look for any with a differance of pre-conditioned maximum
                    if (joltages[i] - joltages[j] <= _maxDiff) 
                    {
                        // for each time we find a previous joltage that 
                        // is within the limit; increment the number of 
                        // permutations for current joltage (i) with the 
                        // permutations that was possible for the previous 
                        // joltage (j)
                        permutations[i] += permutations[j];
                    }
                }
            }

            // the last permutation value is the total number of 
            // permutations given the input provided
            return new PartialResult(2, permutations.Last(), Stopwatch.Elapsed);
        }
    }
}
