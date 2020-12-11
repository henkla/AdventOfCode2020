using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2020.Domain.Day11
{
    public class SeatingSystem : BaseChallenge
    {
        private IEnumerable<string> _input;
        private Tile[][] _seats;

        public enum Tile
        {
            Floor,
            Available,
            Occupied
        }

        private class Direction
        {
            public bool North { get; }
            public bool East { get; }
            public bool South { get; }
            public bool West { get; }

            public bool NE => North && East;
            public bool SE => South && East;
            public bool SW => South && West;
            public bool NW => North && West;


            public Direction(Tile[][] seats, int x, int y)
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
            // start out with a clean state
            var seats = _seats;
            var count = 0;

            // continue until the states match
            while (true)
            {
                var oldState = seats.Select(l => l.Select(x => x).ToList()).ToList();
                var newState = Iterate(seats);

                Debug.WriteLine($"{count++} iterations completed");

                // does the states match?
                if (Enumerable.SequenceEqual(oldState.SelectMany(a => a), newState.SelectMany(b => b)))
                {
                    // we have our result - break out of forever-loop
                    Result.First = newState.Sum(y => y.Count(x => x == Tile.Occupied));
                    return;
                }

                // update the seat state with the new (altered) one
                seats = newState;
            }
        }

        public Tile[][] Iterate(Tile[][] seats)
        {
            // this is the seat-state we will be operating on
            var newState = seats.Select(l => l.Select(c => c).ToArray()).ToArray();

            foreach (var yPos in Enumerable.Range(0, seats.Length))
            {
                foreach (var xPos in Enumerable.Range(0, seats[0].Length))
                {
                    // this is the current seat
                    var current = newState[yPos][xPos];

                    // find out possible directions from current xy
                    var go = new Direction(seats, xPos, yPos);

                    // find out how many taken seats there are in the surroundings
                    var neighbours = new[]
                    {
                        go.North && seats[yPos - 1][xPos] == Tile.Occupied,
                        go.South && seats[yPos + 1][xPos] == Tile.Occupied,
                        go.West && seats[yPos][xPos - 1] == Tile.Occupied,
                        go.East && seats[yPos][xPos + 1] == Tile.Occupied,
                        go.NW && seats[yPos - 1][xPos - 1] == Tile.Occupied,
                        go.NE && seats[yPos - 1][xPos + 1] == Tile.Occupied,
                        go.SW && seats[yPos + 1][xPos - 1] == Tile.Occupied,
                        go.SE && seats[yPos + 1][xPos + 1] == Tile.Occupied
                    };
                    
                    // how many occupied seats are there among the neighbours?
                    var occupieds = neighbours.Count(x => x);

                    // - If a seat is occupied(#) and four or more seats adjacent to it are also 
                    //   occupied, the seat becomes empty.
                    if (current == Tile.Occupied && occupieds >= 4)
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
                    };
                }
            }

            return newState;
        }

        protected override void SolveSecond()
        {
        }
    }
}
