using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Domain.Day14
{
    public class DockingData : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt");
        }

        protected override void SolveFirst()
        {
            var register = new Dictionary<int, string>();
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
                        var binary = ApplyMask(int.Parse(RHS), mask);
                        var key = ExtractDigitsFromString(LHS);
                        AddToRegister(register, key, binary);
                        break;
                }
            }

            register.Select(kv => kv.Value).ToList().ForEach(v => Result.First += Convert.ToInt64(v, 2));

        }

        private int ExtractDigitsFromString(string source)
        {
            return int.Parse(Regex.Match(source, @"\d+").Value);
        }

        private void AddToRegister(IDictionary<int, string> register, int key, string value)
        {
            if (register.ContainsKey(key))
                register[key] = value;
            else
                register.Add(key, value);
        }

        private string ConvertIntToBinary(int source, int length)
        {
            return Convert.ToString(source, 2).PadLeft(length, '0');
        }

        private string ApplyMask(int value, string mask)
        {
            var result = Enumerable.Repeat('0', mask.Length).ToArray();
            var binary = ConvertIntToBinary(value, 36);

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

        private string ApplyMask2(int value, string mask)
        {
            var result = Enumerable.Repeat('0', mask.Length).ToArray();
            var binary = ConvertIntToBinary(value, 36);

            for (int index = binary.Length - 1; index >= 0; index--)
            {
                // If the bitmask bit is 0, the corresponding memory address bit is unchanged
                if (Equals(mask[index], '0'))
                {
                    result[index] = binary[index];
                }
                // If the bitmask bit is 1, the corresponding memory address bit is overwritten with 1
                // If the bitmask bit is X, the corresponding memory address bit is floating.
                else
                {
                    result[index] = mask[index];
                }
            }

            return new string(result);
        }

        private (string LHS, string RHS) ParseInstruction(string line)
        {
            var instruction = line.Split('=').ToArray();
            return (instruction[0].Trim(), instruction[1].Trim());
        }

        protected override void SolveSecond()
        {
            var register = new Dictionary<string, string>();
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
                        var key = ApplyMask2(ExtractDigitsFromString(LHS), mask);
                        AddToRegister2(register, key, RHS);
                        break;
                }
            }

            register.Select(kv => kv.Value).ToList().ForEach(v => Result.Second += Convert.ToInt64(v));
        }

        private void AddToRegister2(Dictionary<string, string> register, string key, string value)
        {
            var allKeys = new List<string>() { key };
            
            while (allKeys.Any(s => s.Contains('X')))
            {
                var floatingKey = allKeys.First(s => s.Contains('X'));
                var expandedKey = new StringBuilder(floatingKey);

                expandedKey[floatingKey.IndexOf('X')] = '0';
                allKeys.Add(expandedKey.ToString());
                
                expandedKey[floatingKey.IndexOf('X')] = '1';
                allKeys.Add(expandedKey.ToString());
                
                allKeys.Remove(floatingKey);
            }

            foreach (var k in allKeys)
            {
                if (register.ContainsKey(k))
                    register[key] = value;
                else
                    register.Add(k, value);
            }
        }
    }
}
