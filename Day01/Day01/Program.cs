using Day01.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        private const int _target = 2020;

        static void Main(string[] args)
        {
            var inputReader = new InputReader();
            // order input by ascending
            var actualSortedInput = inputReader.GetInputCollection("input - Copy.txt", true);
            //Part1(actualSortedInput);
            Part2(actualSortedInput);
        }

        private static void Part1(IEnumerable<int> actualSortedInput)
        {
            Console.WriteLine("Part 1:\n");

            // loop through every number
            foreach (var number in actualSortedInput)
            {
                Console.WriteLine($"Checking {number}...");

                var desiredPair = _target - number;
                if (actualSortedInput.Contains(desiredPair))
                {
                    Console.WriteLine($"The pair than goes with {number} is {desiredPair} and the product of those are {number * desiredPair}!");
                    break;
                }
            }
        }

        /*
        The Elves in accounting are thankful for your help; one of them even offers you a starfish 
        coin they had left over from a past vacation. They offer you a second one if you can find 
        three numbers in your expense report that meet the same criteria.

        Using the above example again, the three entries that sum to 2020 are 979, 366, and 675. 
        Multiplying them together produces the answer, 241861950.

        In your expense report, what is the product of the three entries that sum to 2020?
         */
        private static void Part2(IEnumerable<int> actualSortedInput)
        {
            Console.WriteLine("\nPart 2:\n");
            
            foreach (var num1 in actualSortedInput)
            {
                foreach (var num2 in actualSortedInput)
                {
                    var desiredPair = _target - num1 - num2;
                    if (actualSortedInput.Contains(desiredPair))
                    {
                        Console.WriteLine($"The pair than goes with {num1} and {num2} is {desiredPair} and the product of those are {num1 * num2 * desiredPair}!");
                        break;
                    }
                }
            }
        }
    }
}
