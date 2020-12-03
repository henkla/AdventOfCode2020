using AdventOfCode2020.Library;
using System;
using System.Collections.Generic;

namespace Day03
{
    public enum Square
    {
        Open = 0,
        Tree = 1
    }

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

            // operate on input?

            Part1(input);
            //Part2(input);
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

            var numberOfTrees = 0;

            // given conditions
            var xTravel = 3;
            var yTravel = 1;

            // set starting position
            var xPosition = xTravel;
            var yPosition = yTravel;

            try
            {
                while (yPosition < map.Length)
                {
                    // get terrain for current position
                    var currentTerrain = map[yPosition][xPosition];

                    // check the current terrain
                    if (currentTerrain == '#')
                    {
                        // it's a tree, so add to the count
                        numberOfTrees++;

                        // mark the type of terrain on map
                        map[yPosition][xPosition] = 'X';
                    }
                    else
                    {
                        // mark the type of terrain on map
                        map[yPosition][xPosition] = 'O';
                    }

                    // set next position
                    xPosition += xTravel;
                    yPosition += yTravel;

                    // check if reaching boundaries in x-path
                    if (xPosition >= map[yPosition - 1].Length)
                    {
                        xPosition -= map[xPosition].Length;
                    }
                }

                Console.WriteLine($"There are {numberOfTrees} number of trees in the chosen path!");
            }
            catch (Exception)
            {
                Console.WriteLine("You fool! Did you just walk outside the boundaries of the map?");
            }
            finally
            {
                // just for the fun of it
                PrintTraversedMap(map);
            }

            Console.ReadKey();
        }

        private static void PrintTraversedMap(char[][] map)
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    Console.Write(map[y][x]);
                }

                Console.WriteLine();
            }
        }

        private static void Part2(IEnumerable<string> input)
        {
            throw new NotImplementedException();
            Console.WriteLine("Part 2:\n");
            Console.ReadKey();
        }
    }
}
