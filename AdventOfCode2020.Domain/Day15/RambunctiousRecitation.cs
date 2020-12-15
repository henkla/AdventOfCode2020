using AdventOfCode2020.Tools;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day15
{
    public class RambunctiousRecitation : BaseChallenge
    {
        private int[] _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt")
                .Single()
                .Split(',')
                .Select(s => int.Parse(s))
                .ToArray();
        }

        protected override PartialResult SolveFirst()
        {
            return new PartialResult(1, PlayGame(2020), Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            return new PartialResult(2, PlayGame(30000000), Stopwatch.Elapsed);
        }

        private long PlayGame(int turnLimit)
        {
            var history = new Dictionary<long, (long Times, long LastTurn, long EarlierTurn)>();
            _input.ToList().ForEach(i => history.AddOrUpdate(i, (0, 0, 0)));

            long lastSpoken = 0;
            foreach (int turn in Enumerable.Range(0, turnLimit))
            {
                long spoken = _input[turn % _input.Length];

                if (history[lastSpoken].Times == 1 && turn > _input.Length)
                {
                    spoken = 0;
                }
                else if (history[lastSpoken].Times > 1 && turn > _input.Length)
                {
                    spoken = history[lastSpoken].LastTurn - history[lastSpoken].EarlierTurn;
                    if (!history.ContainsKey(spoken))
                    {
                        history.Add(spoken, (0, 0, 0));
                    }
                }

                history[spoken] = (history[spoken].Times + 1, turn, history[spoken].LastTurn);
                lastSpoken = spoken;
            }

            return lastSpoken;
        }
    }
}
