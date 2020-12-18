using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day17
{
    public class ConwayCubes : BaseChallenge
    {
        private string[] _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("example.txt").ToArray();
        }

        public class Cube
        {
            public int Z { get; private set; }
            public int Y { get; private set; }
            public int X { get; private set; }
            public char State { get; set; }

            public Cube(int x, int y, int z, char state)
            {
                X = x;
                Y = y;
                Z = z;
                State = state;
            }

            public IEnumerable<Cube> GetNeighbours(ICollection<Cube> allCubes)
            {
                var neighbours = new List<Cube>();

                foreach (var cube in allCubes)
                {
                    if (cube.X == X + 1 || cube.X == X - 1)
                    {
                        neighbours.Add(cube);
                    }
                    else if (cube.Y == Y + 1 || cube.Y == Y - 1)
                    {
                        neighbours.Add(cube);
                    }
                    if (cube.Z == Z + 1 || cube.Z == Z - 1)
                    {
                        neighbours.Add(cube);
                    }
                }

                return neighbours;
            }

            internal ICollection<Cube> Update(ICollection<Cube> allCubes)
            {
                var min = allCubes.Select(c => c.X).Min();
                var max = allCubes.Select(c => c.X).Max();

                if (!allCubes.Any(c => c.X == X + 1))
                {
                    foreach (var y in Enumerable.Range(min, max))
                    {
                        foreach (var z in Enumerable.Range(min, max))
                        {
                            allCubes.Add(new Cube(X + 1, y, z, '.'));
                        }
                    }
                }

                if (!allCubes.Any(c => c.X == X - 1))
                {
                    foreach (var y in Enumerable.Range(min, max))
                    {
                        foreach (var z in Enumerable.Range(min, max))
                        {
                            allCubes.Add(new Cube(X - 1, y, z, '.'));
                        }
                    }
                }

                if (!allCubes.Any(c => c.Y == Y + 1))
                {
                    foreach (var x in Enumerable.Range(min, max))
                    {
                        foreach (var z in Enumerable.Range(min, max))
                        {
                            allCubes.Add(new Cube(X, Y + 1, z, '.'));
                        }
                    }
                }

                if (!allCubes.Any(c => c.Y == Y - 1))
                {
                    foreach (var x in Enumerable.Range(min, max))
                    {
                        foreach (var z in Enumerable.Range(min, max))
                        {
                            allCubes.Add(new Cube(X, Y - 1, z, '.'));
                        }
                    }
                }

                if (!allCubes.Any(c => c.Z == Z + 1))
                {
                    foreach (var x in Enumerable.Range(min, max))
                    {
                        foreach (var z in Enumerable.Range(min, max))
                        {
                            allCubes.Add(new Cube(X, Y, z + 1, '.'));
                        }
                    }
                }

                if (!allCubes.Any(c => c.Z == Z - 1))
                {
                    foreach (var x in Enumerable.Range(min, max))
                    {
                        foreach (var z in Enumerable.Range(min, max))
                        {
                            allCubes.Add(new Cube(X, Y, z - 1, '.'));
                        }
                    }
                }

                return allCubes;
            }
        }

        protected override PartialResult SolveFirst()
        {
            // start out with a clean state
            var cubes = ParseInput(_input);
            var newState = cubes;
            var oldState = cubes;

            foreach (var iteration in Enumerable.Range(0, 6))
            {
                var activeCubes = cubes.Count(c => c.State == '#');

                oldState = cubes.Select(c => c).ToList();
                newState = Iterate(oldState);
                cubes = newState.Select(c => c).ToList();


            }

            return new PartialResult(1, cubes.Count(c => c.State == '#'), Stopwatch.Elapsed);
        }

        private ICollection<Cube> Iterate(ICollection<Cube> state)
        {
            var changingState = state.Select(c => c).ToList();
            changingState.ForEach(c => c.Update(state));
            foreach (var cube in changingState)
            {

                var alive = cube.GetNeighbours(changingState).Where(c => c.State == '#').Count();

                // If a cube is active and exactly 2 or 3 of its neighbors are also active, the 
                // cube remains active. Otherwise, the cube becomes inactive.
                if (cube.State == '#' && alive < 2 || alive > 3)
                {
                    cube.State = '.';
                }
                // If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes 
                // active. Otherwise, the cube remains inactive.
                else if (cube.State == '.' && alive == 3)
                {
                    cube.State = '#';
                }
            }

            //var min = state.Select(c => c.X).Min() - 1;
            //var max = state.Select(c => c.X).Max() + 1;

            //var newCubes = new List<Cube>();
            //newCubes.Add(new Cube(changingState, min, min, min, '.'));
            //newCubes.Add(new Cube(changingState, min, min, min, '.'));
            //newCubes.Add(new Cube(changingState, min, min, min, '.'));


            return changingState;
        }

        protected override PartialResult SolveSecond()
        {
            return new PartialResult(2, 0, Stopwatch.Elapsed);
        }

        private ICollection<Cube> ParseInput(string[] input)
        {
            var cubes = new List<Cube>();

            foreach (var y in Enumerable.Range(0, input.Length))
            {
                var row = input[y];
                foreach (var x in Enumerable.Range(0, row.Length))
                {
                    var cube = row[x];
                    cubes.Add(new Cube(x - 1, y - 1, -1, '.'));
                    cubes.Add(new Cube(x - 1, y - 1, 0, cube));
                    cubes.Add(new Cube(x - 1, y - 1, 1, '.'));
                }
            }

            return cubes;
        }
    }
}
