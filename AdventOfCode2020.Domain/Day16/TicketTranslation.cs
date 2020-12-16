using AdventOfCode2020.Tools;
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
            var (Rules, _, NearbyTickets) = ParseInput(_input);

            var result = 0L;
            NearbyTickets.SelectMany(l => l.Select(n => n))
                         .ToList()
                         .ForEach(n => result += Rules.Values.SelectMany(l => l.Select(r => r))
                                                      .All(r => !InRange(n, r)) ? n : 0);

            return new PartialResult(1, result, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            var (Rules, MyTicket, NearbyTickets) = ParseInput(_input);
            
            // add my ticket to the nearby tickets
            NearbyTickets.Add(MyTicket);

            // remove the invalid tickets from collection of nearby's
            NearbyTickets.SelectMany(l => l.Select(n => n))
                .Where(n => Rules.Values.SelectMany(l => l.Select(r => r)).All(r => !InRange(n, r)))
                .ToList()
                .ForEach(n => NearbyTickets.RemoveAll(r => r.Contains(n)));

            // match each ticket column (number index) with each corresponding rule
            // there will be columns that matches several rules
            var rules = new Dictionary<string, List<int>>();
            foreach (var index in Enumerable.Range(0, Rules.Count()))
            {
                var column = NearbyTickets.Select(ticket => ticket[index]);

                foreach (var rule in Rules)
                {
                    if (column.All(n => rule.Value.Any(r => InRange(n, r))))
                    {
                        if (rules.ContainsKey(rule.Key))
                        {
                            rules[rule.Key].Add(index);
                        }
                        else
                        {
                            rules.Add(rule.Key, new List<int> { index });
                        }
                    }
                }
            }

            // as long as there are rules which can be applied to more than one ticket column,
            // eliminate them one by one until there are only rules that applies to one
            // ticket column only (hence the use of single - fail fast if something un-
            // predicted should occur)
            while (rules.Select(r => r.Value).Any(l => l.Count() > 1))
            {
                foreach (var rule in rules.Where(r => r.Value.Count() == 1))
                {
                    rules.Where(kv => kv.Value.Contains(rule.Value.Single()) && kv.Value.Count() > 1)
                         .Select(r => r.Key)
                         .ToList()
                         .ForEach(key => rules.Where(r => Equals(key, r.Key))
                                              .Select(r => r.Value)
                                              .ToList()
                                              .ForEach(l => l.Remove(rule.Value.Single())));
                }
            }

            // it's time to extract out result - find the rules who begins with the word
            // "departure". for each of those corresponding indexes - multiply the ticket
            // value for each said index from MyTicket
            var result = 1L;
            rules.Where(r => r.Key.StartsWith("departure"))
                .SelectMany(r => r.Value.Select(n => n))
                .ToList()
                .ForEach(n => result *= MyTicket[n]);

            return new PartialResult(2, result, Stopwatch.Elapsed);
        }

        private (IDictionary<string, List<Range>> Rules, int[] MyTicket, List<int[]> NearbyTickets) ParseInput(string[] input)
        {
            var emptyLine1 = input.ToList().IndexOf("");
            var emptyLine2 = input.ToList().IndexOf("", emptyLine1 + 1);

            var parsedMyTicket = input[emptyLine2 - 1].Split(",").Select(s => int.Parse(s)).ToArray();
            var parsedRules = new Dictionary<string, List<Range>>();
            foreach (var index in Enumerable.Range(0, emptyLine1))
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
            foreach (var index in Enumerable.Range(emptyLine2 + 2, input.Length - (emptyLine2 + 2)))
            {
                parsedNearbyTickets.Add(input[index].Split(",").Select(s => int.Parse(s)).ToArray());
            }

            return (parsedRules, parsedMyTicket, parsedNearbyTickets);
        }

        private bool InRange(int n, Range r)
        {
            return n >= r.Start.Value && n <= r.End.Value;
        }
    }
}
