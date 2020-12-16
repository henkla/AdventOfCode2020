using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day16
{
    public class TicketTranslation : BaseChallenge
    {
        private string[] _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt").ToArray();
        }

        protected override PartialResult SolveFirst()
        {
            var (Rules, MyTicket, NearbyTickets) = ParseInput(_input);

            var result = 0;
            NearbyTickets.SelectMany(n => n.Select(num => num))
                         .ToList()
                         .ForEach(n => result += Rules.Values.SelectMany(l => l.Select(r => r))
                                                      .All(r => n < r.Start.Value || n > r.End.Value) ? n : 0);

            return new PartialResult(1, result, Stopwatch.Elapsed);
        }

        private (IDictionary<string, List<Range>> Rules, int[] MyTicket, List<int[]> NearbyTickets) ParseInput(string[] input)
        {
            var firstSeparator = input.ToList().IndexOf("");
            var secondSeparator = input.ToList().IndexOf("", firstSeparator + 1);

            var parsedMyTicket = input[secondSeparator - 1].Split(",").Select(s => int.Parse(s)).ToArray();
            var parsedRules = new Dictionary<string, List<Range>>();
            foreach (var index in Enumerable.Range(0, firstSeparator))
            {
                var ruleName = input[index].Split(": ")[0].Trim();
                var ruleValue = input[index].Split(": ")[1].Trim();
                var rules = ruleValue.Split(" or ");

                var ranges = new List<Range>();
                foreach (var r in rules)
                {
                    var start = int.Parse(r.Split('-')[0]);
                    var end = int.Parse(r.Split('-')[1]);
                    ranges.Add(new Range(start, end));
                }

                parsedRules.Add(ruleName, ranges);
            }

            var parsedNearbyTickets = new List<int[]>();
            foreach (var index in Enumerable.Range(secondSeparator + 2, input.Length - (secondSeparator + 2)))
            {
                parsedNearbyTickets.Add(input[index].Split(",").Select(s => int.Parse(s)).ToArray());
            }

            return (parsedRules, parsedMyTicket, parsedNearbyTickets);
        }

        protected override PartialResult SolveSecond()
        {
            return new PartialResult(2, 0, Stopwatch.Elapsed);
        }
    }
}
