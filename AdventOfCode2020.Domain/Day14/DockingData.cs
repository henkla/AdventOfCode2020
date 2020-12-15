﻿using AdventOfCode2020.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Domain.Day14
{
    public class DockingData : BaseChallenge
    {
        private Converter _converter;
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _converter = new Converter();
            _input = InputReader.ReadFile("input.txt");
        }

        protected override void SolveFirst()
        {
            var register = new Dictionary<long, string>();
            var mask = string.Empty;

            foreach (var line in _input)
            {
                var (LHS, RHS) = ParseInstruction(line);

                switch (LHS)
                {
                    case "mask":
                        mask = RHS;
                        break;
                    default:
                        register.AddOrUpdate(LHS.ExtractNumbers(), SimpleMask(int.Parse(RHS), mask));
                        break;
                }
            }

            register.Select(kv => kv.Value).ToList().ForEach(v => Result.First += _converter.Base2ToBase10(v));
        }

        protected override void SolveSecond()
        {
            var register = new Dictionary<long, string>();
            var mask = string.Empty;

            foreach (var line in _input)
            {
                var (LHS, RHS) = ParseInstruction(line);

                switch (LHS)
                {
                    case "mask":
                        mask = RHS;
                        break;
                    default:
                        ExpandFloatingKey(AdvancedMask(LHS.ExtractNumbers(), mask))
                            .ForEach(k => register.AddOrUpdate(k, RHS));
                        break;
                }
            }

            register.Select(kv => kv.Value).ToList().ForEach(v => Result.Second += long.Parse(v));
        }

        private (string LHS, string RHS) ParseInstruction(string line)
        {
            var instruction = line.Split('=').ToArray();
            return (instruction[0].Trim(), instruction[1].Trim());
        }

        private string SimpleMask(int value, string mask)
        {
            var result = Enumerable.Repeat('0', mask.Length).ToArray();
            var binary = _converter.Base10ToBase2(value, 36);

            for (int index = binary.Length - 1; index >= 0; index--)
            {
                if (Equals(mask[index], 'X'))
                {
                    result[index] = binary[index];
                }
                else
                {
                    result[index] = mask[index];
                }
            }

            return new string(result);
        }

        private string AdvancedMask(int value, string mask)
        {
            var result = Enumerable.Repeat('0', mask.Length).ToArray();
            var binary = _converter.Base10ToBase2(value, 36);

            for (int index = binary.Length - 1; index >= 0; index--)
            {
                if (Equals(mask[index], '0'))
                {
                    result[index] = binary[index];
                }
                else
                {
                    result[index] = mask[index];
                }
            }

            return new string(result);
        }

        private List<long> ExpandFloatingKey(string floatingKey)
        {
            var keys = new List<string>() { floatingKey };

            while (keys.Any(key => key.Contains('X')))
            {
                var xKey = keys.First(key => key.Contains('X'));
                var newKey = new StringBuilder(xKey);

                newKey[xKey.IndexOf('X')] = '0';
                keys.Add(newKey.ToString());

                newKey[xKey.IndexOf('X')] = '1';
                keys.Add(newKey.ToString());

                keys.Remove(xKey);
            }

            return keys.Select(key => _converter.Base2ToBase10(key)).ToList();
        }
    }
}
