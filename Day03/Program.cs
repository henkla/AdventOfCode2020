using AdventOfCode2020.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Day03
{
    class Program
    {
        /*
         --- Day 3: Toboggan Trajectory ---
        With the toboggan login problems resolved, you set off toward the airport. 
        While travel by toboggan might be easy, it's certainly not safe: there's 
        very minimal steering and the area is covered in trees. You'll need to 
        see which angles will take you near the fewest trees.

        Due to the local geology, trees in this area only grow on exact integer 
        coordinates in a grid. You make a map (your puzzle input) of the open 
        squares (.) and trees (#) you can see. For example:

        <input_example.txt>

        These aren't the only trees, though; due to something you read about once 
        involving arboreal genetics and biome stability, the same pattern repeats 
        to the right many times:
         */
        static void Main(string[] args)
        {
            var inputHelper = new InputHelper();
            var input = inputHelper.GetInputAsGrid("input.txt");

            Part1(input);
            Part2(input);
        }

        /*
        You start on the open square (.) in the top-left corner and need to reach 
        the bottom (below the bottom-most row on your map).

        The toboggan can only follow a few specific slopes (you opted for a cheaper 
        model that prefers rational numbers); start by counting all the trees you 
        would encounter for the slope right 3, down 1:

        From your starting position at the top-left, check the position that is 
        right 3 and down 1. Then, check the position that is right 3 and down 1 
        from there, and so on until you go past the bottom of the map.

        The locations you'd check in the above example are marked here with O where 
        there was an open square and X where there was a tree:

        <input_example2.txt>

        In this example, traversing the map using this slope would cause you to 
        encounter 7 trees.

        Starting at the top-left corner of your map and following a slope of 
        right 3 and down 1, how many trees would you encounter?
         */
        private static void Part1(char[][] map)
        {
            Console.WriteLine("Part 1:\n");

            var numberOfTrees = TraverseTheMap(map, 3, 1);
            Console.WriteLine($"There are {numberOfTrees} number of trees!");

            Console.ReadKey();
        }

        /*
        --- Part Two ---
        Time to check the rest of the slopes - you need to minimize 
        the probability of a sudden arboreal stop, after all.

        Determine the number of trees you would encounter if, for each 
        of the following slopes, you start at the top-left corner and 
        traverse the map all the way to the bottom:

        Right 1, down 1.
        Right 3, down 1. (This is the slope you already checked.)
        Right 5, down 1.
        Right 7, down 1.
        Right 1, down 2.
        In the above example, these slopes would find 2, 7, 3, 4, and 
        2 tree(s) respectively; multiplied together, these produce the 
        answer 336.

        What do you get if you multiply together the number of trees 
        encountered on each of the listed slopes?
         */
        private static void Part2(char[][] map)
        {
            Console.WriteLine("Part 2:\n");

            // store number of trees for each traversed slope
            var totalNumberOfTrees = new List<long>();

            // the given path to travel for each slope
            var travelConditions = new List<int[]>
            {
                new int[] { 1, 1 },
                new int[] { 3, 1 },
                new int[] { 5, 1 },
                new int[] { 7, 1 },
                new int[] { 1, 2 },
            };

            // the number of trees for each traversed slope
            foreach (var condition in travelConditions)
            {
                var currentNumberOfTrees = TraverseTheMap(map, condition[0], condition[1]);
                Console.WriteLine($"For x={condition[0]} and y={condition[1]}, there are {currentNumberOfTrees} trees.");
                totalNumberOfTrees.Add((long)currentNumberOfTrees);
            }

            // get the product of the total number of trees
            long product = 1; // initial product must be set to 1
            foreach (var numberOfTrees in totalNumberOfTrees)
            {
                product *= numberOfTrees;
            }

            Console.WriteLine($"The product of all the trees for each traversed slope are {product}.");
            Console.ReadKey();
        }

        private static int TraverseTheMap(char[][] map, int xTravel, int yTravel)
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
            catch (Exception)
            {
                Console.WriteLine("You fool! Did you just walk outside the boundaries of the map?");
            }

            return numberOfTrees;
        }
    }
}
