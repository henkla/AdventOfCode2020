using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day05
{
    internal class BinaryBoarding : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = _inputHelper.GetInputAsLines("input.txt");
        }

        protected override void SolveFirst()
        {
            // store the current highest Id, since that's what
            // part 1 is all about
            var highestId = 0;

            // go thorugh each input line (whic is supposed to
            // represent a boarding pass) and parse it
            foreach (var line in _input)
            {
                // retrieve row, column and id from each boarding pass
                (int Row, int Column, int Id) boardingPass = ParseBoardingPass(line.Trim());

                // check if the current id is a new highest
                if (boardingPass.Id > highestId)
                {
                    highestId = boardingPass.Id;
                }
            }

            _result.First = highestId;
        }

        private (int Row, int Column, int Id) ParseBoardingPass(string line)
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
                        throw new System.Exception($"Letter {letter} is not valid.");
                }
            }

            // there should be only 1 element left in each array - I'm
            // going to be brave and assume that this is a fact for the
            // upcoming part as well
            return (rowsOnPlane[0], columnsInRow[0], (rowsOnPlane[0] * idFactor + columnsInRow[0]));
        }

        private int[] DivideAndKeepLastPart(int divisor, int[] array)
        {
            return array.Skip(array.Length / divisor).ToArray();
        }

        private int[] DivideAndKeepFirstPart(int divisor, int[] array)
        {
            return array.Take(array.Length / divisor).ToArray();
        }

        protected override void SolveSecond()
        {
            // we need to store each seat id in a list so that
            // we later on can figure out the missing element
            // in the sequence on id's
            var seatIds = new List<int>();

            // go thorugh each input line (which is supposed to
            // represent a boarding pass) and parse it
            foreach (var line in _input)
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
            var missingId = Enumerable.Range(8, seatIds.Count() - 8)
                .Except(seatIds)
                .Single();

            _result.Second = missingId;
        }
    }
}
