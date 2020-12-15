using AdventOfCode2020.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day14
{
    public class DockingData : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt");
        }

        protected override PartialResult SolveFirst()
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

            long result = 0;
            register.Select(kv => kv.Value).ToList().ForEach(v => result += Converter.Base2ToBase10(v));
            return new PartialResult(1, result, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
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

            long result = 0;
            register.Select(kv => kv.Value).ToList().ForEach(v => result += long.Parse(v));
            return new PartialResult(2, result, Stopwatch.Elapsed);
        }

        private (string LHS, string RHS) ParseInstruction(string line)
        {
            var instruction = line.Split('=');
            return (instruction[0].Trim(), instruction[1].Trim());
        }

        private List<long> ExpandFloatingKey(string floatingKey)
        {
            var allKeys = new List<string>() { floatingKey };

            while (allKeys.Any(s => s.Contains('X')))
            {
                var xKey = allKeys.First(s => s.Contains('X'));
                var expandedKey = new StringBuilder(xKey);

                expandedKey[xKey.IndexOf('X')] = '0';
                allKeys.Add(expandedKey.ToString());

                expandedKey[xKey.IndexOf('X')] = '1';
                allKeys.Add(expandedKey.ToString());

                allKeys.Remove(xKey);
            }

            return allKeys.Select(key => Converter.Base2ToBase10(key)).ToList();
        }

        private string SimpleMask(int value, string mask)
        {
            var result = Enumerable.Repeat('0', mask.Length).ToArray();
            var binary = Converter.Base10ToBase2(value, 36);

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
            var binary = Converter.Base10ToBase2(value, 36);
            
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
    }
}
