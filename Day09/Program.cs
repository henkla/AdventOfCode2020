using AdventOfCode2020.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day09
{
    class Program
    {
        /*
        With your neighbor happily enjoying their video game, you turn your attention 
        to an open data port on the little screen in the seat in front of you.

        Though the port is non-standard, you manage to connect it to your computer 
        through the clever use of several paperclips. Upon connection, the port outputs 
        a series of numbers (your puzzle input).
        
        The data appears to be encrypted with the eXchange-Masking Addition System (XMAS) 
        which, conveniently for you, is an old cypher with an important weakness.
        
        XMAS starts by transmitting a preamble of 25 numbers. After that, each number you 
        receive should be the sum of any two of the 25 immediately previous numbers. The 
        two numbers will have different values, and there might be more than one such pair.
        
        For example, suppose your preamble consists of the numbers 1 through 25 in a 
        random order. To be valid, the next number must be the sum of two of those numbers:
        
        26 would be a valid next number, as it could be 1 plus 25 (or many other pairs, 
            like 2 and 24).
        49 would be a valid next number, as it is the sum of 24 and 25.
        100 would not be valid; no two of the previous 25 numbers sum to 100.
        50 would also not be valid; although 25 appears in the previous 25 numbers, the 
            two numbers in the pair must be different.
        
        Suppose the 26th number is 45, and the first number (no longer an option, as 
        it is more than 25 numbers ago) was 20. Now, for the next number to be valid, 
        there needs to be some pair of numbers among 1-19, 21-25, or 45 that add up to it:
        
        26 would still be a valid next number, as 1 and 25 are still within the previous 
            25 numbers.
        65 would not be valid, as no two of the available numbers sum to it.
        64 and 66 would both be valid, as they are the result of 19+45 and 21+45 respectively.
        Here is a larger example which only considers the previous 5 numbers (and 
            has a preamble of length 5):
        
        35
        20
        15
        25
        47
        40
        62
        55
        65
        95
        102
        117
        150
        182
        127
        219
        299
        277
        309
        576
        
        In this example, after the 5-number preamble, almost every number is the sum 
        of two of the previous 5 numbers; the only number that does not follow this 
        rule is 127.
         */
        static void Main(string[] args)
        {
            var inputHelper = new InputHelper();
            var input = inputHelper.GetInputAsLines("input.txt");

            var numbers = ParseAllIntegers(input);
            var sizeOfPreample = 25;

            var result = Part1(numbers, sizeOfPreample);
            Part2(numbers, result);
        }

        private static IEnumerable<long> ParseAllIntegers(IEnumerable<string> input)
        {
            var numbers = new List<long>();
            input.ToList().ForEach(s => numbers.Add(long.Parse(s)));
            return numbers;
        }

        /*
        The first step of attacking the weakness in the XMAS data is to find the first 
        number in the list (after the preamble) which is not the sum of two of the 25 
        numbers before it. What is the first number that does not have this property?
         */
        private static long Part1(IEnumerable<long> numbers, int sizeOfPreamble)
        {
            Console.WriteLine("\nPart 1:\n");

            var result = FindSumWithNoTerms(numbers, sizeOfPreamble);

            Console.WriteLine($"For Part 1, the result is {result}");
            Console.ReadKey();

            return result;
        }

        private static long FindSumWithNoTerms(IEnumerable<long> numbers, int sizeOfPreamble)
        {
            long result = 0;
            for (int i = sizeOfPreamble; i < numbers.Count(); i++)
            {
                var preamble = numbers.ToList().GetRange(i - sizeOfPreamble, sizeOfPreamble);
                var number = numbers.ElementAt(i);

                if (ExistsTermsForSum(preamble, number) is false)
                {
                    result = number;
                    break;
                }
            }

            return result;
        }

        private static bool ExistsTermsForSum(IEnumerable<long> preamble, long number)
        {
            foreach (var n in preamble)
            {
                if (preamble.Contains(number - n) && (number - n) != n) 
                    return true;
            }

            return false;
        }

        /*
        The final step in breaking the XMAS encryption relies on the invalid number 
        you just found: you must find a contiguous set of at least two numbers in 
        your list which sum to the invalid number from step 1.

        Again consider the above example:
        
        35
        20
        15
        25
        47
        40
        62
        55
        65
        95
        102
        117
        150
        182
        127
        219
        299
        277
        309
        576

        In this list, adding up all of the numbers from 15 through 40 produces the 
        invalid number from step 1, 127. (Of course, the contiguous set of numbers 
        in your actual list might be much longer.)
        
        To find the encryption weakness, add together the smallest and largest number 
        in this contiguous range; in this example, these are 15 and 47, producing 62.
        
        What is the encryption weakness in your XMAS-encrypted list of numbers?
         */
        private static void Part2(IEnumerable<long> numbers, long target)
        {
            
            Console.WriteLine("\nPart 2:\n");

            var result = FindSumOfMinMaxOfRange(numbers, target);

            Console.WriteLine($"For Part 2, the result is {result}");
            Console.ReadKey();
        }

        private static long FindSumOfMinMaxOfRange(IEnumerable<long> numbers, long targetValue)
        {
            // this is where we'll store our end result
            long result = 0;

            // we need to investigate where the contiguous range of terms begins, starting
            // at zero (all the way up to possibly the index where the target value resides)
            for (int startingIndex = 0; startingIndex < Array.FindIndex(numbers.ToArray(), number => number == targetValue); startingIndex++)
            {
                // this is the sum that we want to equal the target value 
                // later on
                var possibleContiguousSum = numbers.ElementAt(startingIndex);

                // store all terms in a list - we need those for later
                var terms = new List<long>() { possibleContiguousSum };

                // add up all following numbers from the list into the sum up until we
                // either A) receive a sum that as larger than the target (in which case, 
                // the contiguous range was not the correct one) or B) we find a sum that
                // is equal to the target value (in which case, we have the range that we want)
                int sequencialIndex = startingIndex;
                while (possibleContiguousSum < targetValue)
                {
                    // get the element that corresponds with the index
                    var nextTerm = numbers.ElementAt(++sequencialIndex);

                    // remember to store term in list for later, if sum checks out
                    terms.Add(nextTerm);

                    // add this element to the sum
                    possibleContiguousSum += nextTerm;
                }

                // check if the sum is equal to the value that we are looking for
                if (possibleContiguousSum == targetValue) 
                {
                    // the result was supposed to be the smallest term and the
                    // largest term added together
                    result = terms.Min() + terms.Max();
                    break;
                }
            }
            
            // win
            return result;
        }
    }
}
