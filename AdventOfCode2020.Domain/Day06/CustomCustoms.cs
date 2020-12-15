using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day06
{
    public class CustomCustoms : BaseChallenge
    {
        private IEnumerable<IEnumerable<string>> _groups;

        protected override void Initialize()
        {
            var input = InputReader.ReadFile("input.txt");
            _groups = ParseAndGroupAnswersTogether(input);
        }

        protected override PartialResult SolveFirst()
        {
            // for part 1 - run distinct and sum all groups together
            var sum = 0;
            _groups.ToList().ForEach(g =>
            {
                var answers = string.Empty;
                g.ToList().ForEach(a =>
                {
                    answers += a;
                });
                sum += answers.Distinct().Count();
            });

            return new PartialResult(1, sum, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            var sum = 0;
            _groups.ToList().ForEach(g =>
            {
                sum += g.Aggregate((a, b) => a += b).GroupBy(_ => _).Where(_ => _.Count() == g.Count()).Count();
            });

            return new PartialResult(2, sum, Stopwatch.Elapsed);
        }

        private IEnumerable<IEnumerable<string>> ParseAndGroupAnswersTogether(IEnumerable<string> input)
        {
            var groups = new List<IEnumerable<string>>();
            var person = new List<string>();

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    groups.Add(person);
                    person = new List<string>();
                }
                else
                {
                    person.Add(line);
                }
            }

            groups.Add(person);
            return groups;
        }
    }
}