using AdventOfCode2020.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day02
{
    public class PasswordPhilosophy : BaseChallenge
    {
        private IEnumerable<string> _input;
        private int _offset;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt");
            _offset = -1;
        }

        protected override PartialResult SolveFirst()
        {
            var numberOfValidPasswords = 0;
            foreach (var entry in _input)
            {
                var parsedEntry = ParseEntry(entry);
                var occurancesOfCharInPassword = parsedEntry.Password.Count(q => q == parsedEntry.Char);

                if (occurancesOfCharInPassword >= parsedEntry.Numerics[0] && occurancesOfCharInPassword <= parsedEntry.Numerics[1])
                {
                    numberOfValidPasswords++;
                }
            }

            return new PartialResult(1, numberOfValidPasswords, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            var numberOfValidPasswords = 0;
            foreach (var entry in _input)
            {
                var parsedEntry = ParseEntry(entry);
                if (parsedEntry.Password[parsedEntry.Numerics[0] + _offset] == parsedEntry.Char ^ parsedEntry.Password[parsedEntry.Numerics[1] + _offset] == parsedEntry.Char)
                {
                    numberOfValidPasswords++;
                }
            }

            return new PartialResult(2, numberOfValidPasswords, Stopwatch.Elapsed);
        }

        private (char Char, int[] Numerics, string Password) ParseEntry(string entry)
        {
            // 1-6 z: zgkkdzww
            var parsedEntry = entry.Split(':');
            var numerics = Array.ConvertAll(parsedEntry[0].Split(' ')[0].Split('-'), int.Parse);
            return (Convert.ToChar(parsedEntry[0].Split(' ')[1]), numerics, parsedEntry[1].Trim());
        }
    }
}
