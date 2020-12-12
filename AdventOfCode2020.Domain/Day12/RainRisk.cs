using System;
using System.Collections.Generic;

namespace AdventOfCode2020.Domain.Day12
{
    public class RainRisk : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt");
        }

        protected override void SolveFirst()
        {
            var d = 90;
            var x = 0;
            var y = 0;

            foreach (var instruction in _input)
            {
                var (Delta, Direction) = GetDelta(instruction, d);
                
                d = Direction;
                x += Delta.X;
                y += Delta.Y;
            }

            Result.First = Math.Abs(x) + Math.Abs(y);
        }

        private ((int X, int Y) Delta, int Direction) GetDelta(string instruction, int direction)
        {
            var action = instruction[0];
            var value = int.Parse(instruction[1..]);

            return action switch
            {
                'N' => ((value, 0), direction),
                'E' => ((0, value), direction),
                'S' => ((value * -1, 0), direction),
                'W' => ((0, value * -1), direction),
                'L' => ((0, 0), direction - value),
                'R' => ((0, 0), direction + value),
                'F' => (direction % 360) switch
                {
                    0 => ((value, 0), direction),
                    90 => ((0, value), direction),
                    180 => ((value * -1, 0), direction),
                    270 => ((0, value * -1), direction),
                    _ => throw new ArgumentException($"The action {direction} is unknown.")
                },
                _ => throw new ArgumentException($"The action {action} is unknown."),
            };
        }

        protected override void SolveSecond()
        {
            throw new System.NotImplementedException();
        }
    }
}