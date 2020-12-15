using System;
using System.Collections.Generic;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day12
{
    public class RainRisk : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt");
        }

        protected override PartialResult SolveFirst()
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

            return new PartialResult(1, Math.Abs(x) + Math.Abs(y), Stopwatch.Elapsed);
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

        protected override PartialResult SolveSecond()
        {
            var xShip = 0;
            var yShip = 0;
            var xWaypoint = xShip + 10;
            var yWaypoint = yShip + 1;

            foreach (var instruction in _input)
            {
                var (Ship, Waypoint) = GetPositions(instruction, xWaypoint, yWaypoint);

                xShip += Ship.X;
                yShip += Ship.Y;
                xWaypoint = Waypoint.X;
                yWaypoint = Waypoint.Y;
            }

            return new PartialResult(2, Math.Abs(xShip) + Math.Abs(yShip), Stopwatch.Elapsed);
        }

        private ((int X, int Y) Ship, (int X, int Y) Waypoint) GetPositions(string instruction, int x, int y)
        {
            var action = instruction[0];
            var value = int.Parse(instruction[1..]);

            (int X, int Y) Rotate(int x, int y, int theta)
            {
                var (X, Y) = (x, y);
                var rotations = theta / 90;
                
                while (rotations > 0)
                {
                    (X, Y) = (Y, -X);
                    rotations--;
                }
                while (rotations < 0)
                {
                    (X, Y) = (-Y, X);
                    rotations++;
                }

                return (X, Y);
            }

            return action switch
            {
                'N' => ((0, 0), (x, y + value)),
                'E' => ((0, 0), (x + value, y)),
                'S' => ((0, 0), (x, y + (-value))),
                'W' => ((0, 0), (x + (-value), y)),
                'L' => ((0, 0), Rotate(x, y, -value)),
                'R' => ((0, 0), Rotate(x, y, value)),
                'F' => ((x * value, y * value), (x, y)),
                _ => throw new ArgumentException($"The action {action} is unknown."),
            };
        }
    }
}