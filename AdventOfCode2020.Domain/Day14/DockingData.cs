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
            _input = InputReader.ReadFile("example.txt");
        }

        protected override void SolveFirst()
        {
            var memory = new Dictionary<string, string>();
            var mask = string.Empty;

            void AddToMemory(string mem, string value)
            {
                var key = Regex.Match(mem, @"\d+").Value;
                if (memory.ContainsKey(key))
                    memory[key] = value;
                else
                    memory.Add(key, value);
            }

            foreach (var line in _input)
            {
                var (LHS, RHS) = ParseInstruction(line);

                switch (LHS)
                {
                    case "mask":
                        mask = RHS;
                        break;
                    default:
                        AddToMemory(LHS, ApplyMask(RHS, mask));
                        break;
                }
            }

            memory.Select(kv => kv.Value).ToList().ForEach(v => Result.First += Convert.ToInt64(v, 2));
        }
        private string ApplyMask(string mem, string mask)
        {
            var result = Enumerable.Repeat('0', mask.Length).ToArray();
            mem = $"{int.Parse(mem):D36}";
            
            for (int index = mem.Length - 1; index >= 0; index--)
            {
                if (Equals(mask[index], 'X'))
                {
                    result[index] = mem[index];
                }
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
            
        }
    }
}
