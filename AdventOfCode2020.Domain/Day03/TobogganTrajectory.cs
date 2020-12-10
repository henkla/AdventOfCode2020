using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day03
{
    public class TobogganTrajectory : BaseChallenge
    {
        private char[][] _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt")
                .Select(l => l.Select(i => i).ToArray())
                .ToArray();
        }

        protected override void SolveFirst()
        {
            Result.First = TraverseTheMap(_input, 3, 1);
        }

        protected override void SolveSecond()
        {
            // the given path to travel for each slope
            var travelConditions = new List<(int XTravel, int YTravel)>
            {
                ( 1, 1 ),
                ( 3, 1 ),
                ( 5, 1 ),
                ( 7, 1 ),
                ( 1, 2 ),
            };

            // store number of trees for each traversed slope
            var totalNumberOfTrees = new List<long>();

            // the number of trees for each traversed slope
            foreach (var (XTravel, YTravel) in travelConditions)
            {
                var currentNumberOfTrees = TraverseTheMap(_input, XTravel, YTravel);
                totalNumberOfTrees.Add(currentNumberOfTrees);
            }

            // get the product of the total number of trees
            long product = 1; // initial product must be set to 1
            foreach (var numberOfTrees in totalNumberOfTrees)
            {
                product *= numberOfTrees;
            }

            Result.Second = product;
        }

        private long TraverseTheMap(char[][] map, int xTravel, int yTravel)
        {
            // we start out with zero trees
            var numberOfTrees = 0;

            // set starting position
            var xPosition = xTravel;
            var yPosition = yTravel;

            try
            {
                // as long as we haven't reached the bottom of the map
                while (yPosition < map.Length)
                {
                    // check if we have reached the boundaries in x-path
                    if (xPosition >= map[yPosition - 1].Length)
                    {
                        xPosition -= map[yPosition].Length;
                    }

                    // check the current terrain
                    if (map[yPosition][xPosition] == '#')
                    {
                        // it's a tree, so add to the count
                        numberOfTrees++;
                    }

                    // set next position
                    xPosition += xTravel;
                    yPosition += yTravel;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("You fool! Did you just walk outside the boundaries of the map?");
                throw e;
            }

            return numberOfTrees;
        }
    }
}
