using AdventOfCode2020.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day05
{
    class Program
    {
        /*
        --- Day 5: Binary Boarding ---
        You board your plane only to discover a new problem: you dropped 
        your boarding pass! You aren't sure which seat is yours, and all 
        of the flight attendants are busy with the flood of people that 
        suddenly made it through passport control.

        You write a quick program to use your phone's camera to scan all 
        of the nearby boarding passes (your puzzle input); perhaps you 
        can find your seat through process of elimination.
         */
        static void Main(string[] args)
        {
            var inputHelper = new InputHelper();
            var input = inputHelper.GetInputAsLines("input.txt");

            Part1(input);
            Part2(input);
        }

        /*
        Instead of zones or groups, this airline uses binary space partitioning 
        to seat people. A seat might be specified like FBFBBFFRLR, where F means 
        "front", B means "back", L means "left", and R means "right".

        The first 7 characters will either be F or B; these specify exactly one 
        of the 128 rows on the plane (numbered 0 through 127). Each letter tells 
        you which half of a region the given seat is in. Start with the whole 
        list of rows; the first letter indicates whether the seat is in the front 
        (0 through 63) or the back (64 through 127). The next letter indicates 
        which half of that region the seat is in, and so on until you're left with 
        exactly one row.
        
        For example, consider just the first seven characters of FBFBBFFRLR:
        
        Start by considering the whole range, rows 0 through 127.
        F means to take the lower half, keeping rows 0 through 63.
        B means to take the upper half, keeping rows 32 through 63.
        F means to take the lower half, keeping rows 32 through 47.
        B means to take the upper half, keeping rows 40 through 47.
        B keeps rows 44 through 47.
        F keeps rows 44 through 45.
        The final F keeps the lower of the two, row 44.
        The last three characters will be either L or R; these specify exactly one of 
        the 8 columns of seats on the plane (numbered 0 through 7). The same process as 
        above proceeds again, this time with only three steps. L means to keep 
        the lower half, while R means to keep the upper half.
        
        For example, consider just the last 3 characters of FBFBBFFRLR:
        
        Start by considering the whole range, columns 0 through 7.
        R means to take the upper half, keeping columns 4 through 7.
        L means to take the lower half, keeping columns 4 through 5.
        The final R keeps the upper of the two, column 5.
        So, decoding FBFBBFFRLR reveals that it is the seat at row 44, column 5.
        
        Every seat also has a unique seat ID: multiply the row by 8, then add the 
        column. In this example, the seat has ID 44 * 8 + 5 = 357.
        
        Here are some other boarding passes:
        
        BFFFBBFRRR: row 70, column 7, seat ID 567.
        FFFBBBFRRR: row 14, column 7, seat ID 119.
        BBFFBBFRLL: row 102, column 4, seat ID 820.
        As a sanity check, look through your list of boarding passes. 
        What is the highest seat ID on a boarding pass?
         */
        private static void Part1(IEnumerable<string> input)
        {
            Console.WriteLine("\nPart 1:\n");

            // store the current highest Id, since that's what
            // part 1 is all about
            var highestId = 0;

            // go thorugh each input line (whic is supposed to
            // represent a boarding pass) and parse it
            foreach (var line in input)
            {
                // retrieve row, column and id from each boarding pass
                (int Row, int Column, int Id) boardingPass = ParseBoardingPass(line.Trim());

                // check if the current id is a new highest
                if (boardingPass.Id > highestId)
                {
                    highestId = boardingPass.Id;
                    Console.WriteLine($"Id {highestId} is a new highest!");
                }
            }

            Console.WriteLine($"For part 1, the highest Id is {highestId}.");
            Console.ReadKey();
        }

        private static (int Row, int Column, int Id) ParseBoardingPass(string line)
        {
            // this variables represents the given conditions
            var idFactor = 8;
            var divisor = 2;

            // operate on the rows and columns through the
            // use of arrays, initialized with each respective
            // index
            var rowsOnPlane = Enumerable.Range(0, 128).ToArray();
            var columnsInRow = Enumerable.Range(0, 8).ToArray();

            // parse each letter in the boarding pass (the input line)
            foreach (var letter in line)
            {
                switch (letter)
                {
                    // keep the lower part of the rows
                    case 'F':
                        rowsOnPlane = DivideAndKeepFirstPart(divisor, rowsOnPlane);
                        break;
                    // keep the upper part of the rows
                    case 'B':
                        rowsOnPlane = DivideAndKeepLastPart(divisor, rowsOnPlane);
                        break;
                    // keep the lower part of the columns
                    case 'L':
                        columnsInRow = DivideAndKeepFirstPart(divisor, columnsInRow);
                        break;
                    // keep the upper part of the columns
                    case 'R':
                        columnsInRow = DivideAndKeepLastPart(divisor, columnsInRow);
                        break;
                    default:
                        Console.WriteLine("Uuuh Houston, we have a problem in the parser...");
                        Console.WriteLine($"The letter was {letter}.");
                        break;
                }
            }

            // there should be only 1 element left in each array - I'm
            // going to be brave and assume that this is a fact for the
            // upcoming part as well
            return (rowsOnPlane[0], columnsInRow[0], (rowsOnPlane[0] * idFactor + columnsInRow[0]));
        }

        private static int[] DivideAndKeepLastPart(int divisor, int[] array)
        {
            return array.Skip(array.Length / divisor).ToArray();
        }

        private static int[] DivideAndKeepFirstPart(int divisor, int[] array)
        {
            return array.Take(array.Length / divisor).ToArray();
        }

        /*
        --- Part Two ---
        Ding! The "fasten seat belt" signs have turned on. Time 
        to find your seat.

        It's a completely full flight, so your seat should be the 
        only missing boarding pass in your list. However, there's 
        a catch: some of the seats at the very front and back of 
        the plane don't exist on this aircraft, so they'll be 
        missing from your list as well.

        Your seat wasn't at the very front or back, though; the 
        seats with IDs +1 and -1 from yours will be in your list.

        What is the ID of your seat?
         */
        private static void Part2(IEnumerable<string> input)
        {
            Console.WriteLine("\nPart 2:\n");

            // we need to store each seat id in a list so that
            // we later on can figure out the missing element
            // in the sequence on id's
            var seatIds = new List<int>();

            // go thorugh each input line (which is supposed to
            // represent a boarding pass) and parse it
            foreach (var line in input)
            {
                // retrieve row, column and id from each boarding pass
                (int Row, int Column, int Id) boardingPass = ParseBoardingPass(line.Trim());

                // store the id in list above
                seatIds.Add(boardingPass.Id);
            }

            // make sure we operate on a sorted collection
            seatIds.Sort();

            // if my interpretation of the problem is correct,
            // this should be the id that is missing from the
            // sequence, and thus the id that is my own
            var missingId = FindMissingId(seatIds);

            Console.WriteLine($"Fort part 2, the missing id (that is mine) is {missingId}.");
            Console.ReadKey();
        }

        public static int FindMissingId(IEnumerable<int> source)
        {
            // we were told to discard the first and last rows. or rather,
            // we were told that some seats in the front and back were none 
            // existant. i chose to be lazy and interpret it as if first
            // and last row is missing, meaning that the first and last 8
            // seats (i.e id's) respectively will be missing, so ignore those.
            // if everything is fine, this enumeration should contain a single 
            // element, being the missing id that we are looking for. if it 
            // throws an exception - great, then I know!
            return Enumerable.Range(8, source.Count() - 8)
                .Except(source)
                .Single();
        }
    }
}
