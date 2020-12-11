using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day11
{
    public class SeatingSystem : BaseChallenge
    {
        private IEnumerable<string> _input;
        private Tile[][] _seats;

        private enum Tile
        {
            Floor,
            Available,
            Occupied
        }

        private enum Direction
        {
            Negative = -1,
            Neutral = 0,
            Positive = 1
        }

        private class Route
        {
            public bool North { get; }
            public bool East { get; }
            public bool South { get; }
            public bool West { get; }

            public bool NE => North && East;
            public bool SE => South && East;
            public bool SW => South && West;
            public bool NW => North && West;


            public Route(Tile[][] seats, int x, int y)
            {
                North = y > 0;
                East = x <= seats[y].Length - 2;
                South = y <= seats.Length - 2;
                West = x > 0;
            }
        }

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt");
            _seats = _input
                .Select(l => l.Select(c => c switch
                {
                    '.' => Tile.Floor,
                    '#' => Tile.Occupied,
                    'L' => Tile.Available,
                    _ => throw new NotImplementedException()
                }).ToArray())
                .ToArray();
        }

        protected override void SolveFirst()
        {
            Result.First = RunSimulation(GetAdjacentSeatsOfInterest, 4);
        }
        
        protected override void SolveSecond()
        {
            Result.Second = RunSimulation(GetFirstSeatsOfInterest, 5);
        }

        private long RunSimulation(Func<Tile[][], int, int, bool[]> func, int tolerance)
        {
            // start out with a clean state
            var seats = _seats;
            var oldState = seats;
            var newState = seats;

            do
            {
                oldState = seats.Select(l => l.Select(x => x).ToArray()).ToArray();
                newState = Iterate(seats, func, tolerance);
                seats = newState;

            } while (Enumerable.SequenceEqual(oldState.SelectMany(a => a), newState.SelectMany(b => b)) is false);

            return newState.Sum(y => y.Count(x => x == Tile.Occupied));
        }

        private Tile[][] Iterate(Tile[][] seats, Func<Tile[][], int, int, bool[]> seatsOfInterest, int tolerance)
        {
            // this is the seat-state we will be operating on
            var newState = seats.Select(l => l.Select(c => c).ToArray()).ToArray();

            foreach (var yPos in Enumerable.Range(0, seats.Length))
            {
                foreach (var xPos in Enumerable.Range(0, seats[0].Length))
                {
                    // this is the current seat
                    var current = newState[yPos][xPos];

                    // find out how many taken seats there are in the surroundings
                    //var neighbours = GetAdjacentSeatsOfInterest(seats, yPos, xPos);
                    var neighbours = seatsOfInterest.Invoke(seats, yPos, xPos);

                    // how many occupied seats are there among the neighbours?
                    var occupieds = neighbours.Count(x => x);

                    // - If a seat is occupied(#) and four or more seats adjacent to it are also 
                    //   occupied, the seat becomes empty.
                    if (current == Tile.Occupied && occupieds >= tolerance)
                    {
                        // current seat should be available
                        newState[yPos][xPos] = Tile.Available;
                    }
                    // - If a seat is empty(L) and there are no occupied seats adjacent to it,
                    //   the seat becomes occupied.
                    else if (current == Tile.Available && neighbours.All(x => !x))
                    {
                        // current seat should be occupied
                        newState[yPos][xPos] = Tile.Occupied;
                    }
                }
            }

            return newState;
        }

        private bool[] GetAdjacentSeatsOfInterest(Tile[][] seats, int yPos, int xPos)
        {
            // find out possible directions from current xy
            var r = new Route(seats, xPos, yPos);

            // find out how many taken seats there are in the surroundings
            return new[]
            {
                r.North && seats[yPos - 1][xPos] == Tile.Occupied,
                r.South && seats[yPos + 1][xPos] == Tile.Occupied,
                r.West && seats[yPos][xPos - 1] == Tile.Occupied,
                r.East && seats[yPos][xPos + 1] == Tile.Occupied,
                r.NW && seats[yPos - 1][xPos - 1] == Tile.Occupied,
                r.NE && seats[yPos - 1][xPos + 1] == Tile.Occupied,
                r.SW && seats[yPos + 1][xPos - 1] == Tile.Occupied,
                r.SE && seats[yPos + 1][xPos + 1] == Tile.Occupied
            };
        }

        private bool[] GetFirstSeatsOfInterest(Tile[][] seats, int yPos, int xPos)
        {
            // find each occupied seat in each directional vector respectively
            return new[]
            {
                FirstSeat(seats, (xPos, Direction.Neutral), (yPos, Direction.Negative)) == Tile.Occupied,
                FirstSeat(seats, (xPos, Direction.Neutral), (yPos, Direction.Positive)) == Tile.Occupied,
                FirstSeat(seats, (xPos, Direction.Negative), (yPos, Direction.Neutral)) == Tile.Occupied,
                FirstSeat(seats, (xPos, Direction.Positive), (yPos, Direction.Neutral)) == Tile.Occupied,
                FirstSeat(seats, (xPos, Direction.Negative), (yPos, Direction.Negative)) == Tile.Occupied,
                FirstSeat(seats, (xPos, Direction.Positive), (yPos, Direction.Negative)) == Tile.Occupied,
                FirstSeat(seats, (xPos, Direction.Negative), (yPos, Direction.Positive)) == Tile.Occupied,
                FirstSeat(seats, (xPos, Direction.Positive), (yPos, Direction.Positive)) == Tile.Occupied
            };
        }

        private Tile FirstSeat(Tile[][] seats, (int Pos, Direction Dir) x, (int Pos, Direction Dir) y)
        {
            var xCurrent = x.Pos + (int)x.Dir;
            var yCurrent = y.Pos + (int)y.Dir;
            bool WithinBoundaries() => xCurrent >= 0 && yCurrent >= 0 && xCurrent < seats[y.Pos].Length && yCurrent < seats.Length;

            // as long as we don't move out of boundaries
            while (WithinBoundaries())
            {
                // this is the next tile in our directional vector
                var currentTile = seats[yCurrent][xCurrent];

                // we are interested in seats only - either Availble or Occupied
                if (currentTile != Tile.Floor)
                    return currentTile;

                // update position in the desired direction
                xCurrent += (int)x.Dir;
                yCurrent += (int)y.Dir;
            }

            return Tile.Floor;
        }
    }
}
