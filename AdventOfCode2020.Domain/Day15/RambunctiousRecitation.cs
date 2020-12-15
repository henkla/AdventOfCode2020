using AdventOfCode2020.Tools;
using System.Collections.Generic;
using System.Linq;

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

        protected override void SolveFirst()
        {
            Result.First = PlayGame(2020);
        }

        protected override void SolveSecond()
        {
            Result.Second = PlayGame(30000000);
        }

        private long PlayGame(long turnLimit)
        {
            var history = new Dictionary<long, (long Times, long LastTurn, long EarlierTurn)>();
            _input.ToList().ForEach(i => history.AddOrUpdate(i, (0, 0, 0)));

            long lastSpoken = 0;
            long spoken = 0;
            long turn = 0;

            while (turn < turnLimit)
            {
                spoken = _input[turn++ % _input.Length];

                // considering the most recently spoken number:
                // If that was the first time the number has been spoken, the current player says 0.
                if (history[lastSpoken].Times == 1 && turn > _input.Length)
                {
                    spoken = 0;
                }
                // Otherwise, the number had been spoken before; the current player announces how many turns apart the number is from when it was previously spoken.
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
