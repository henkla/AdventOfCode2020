using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Tools
{
    public enum SortBy
    {
        None = 0,
        Ascending = 1,
        Descending = 2
    }

    public class InputHelper
    {
        private readonly string _currentPath;

        public InputHelper(string currentPath)
        {
            _currentPath = currentPath;
        }

        public IEnumerable<string> GetInputAsLines(string inputPath)
        {
            inputPath = _currentPath + inputPath;

            try
            {
                return File.ReadAllLines(inputPath).ToList();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"Filen kunde inte hittas. Kontrollera sökvägen: {inputPath}");
                throw e;
            }
        }

        public char[][] GetInputAsGrid(string inputPath)
        {
            inputPath = _currentPath + inputPath;

            try
            {
                //return File.ReadAllText(inputPath).ToList();
                return File.ReadAllLines(inputPath)
                   .Select(l => l.Select(i => i).ToArray())
                   .ToArray();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"Fel: Filen kunde inte hittas. Kontrollera sökvägen: {inputPath}");
                throw e;
            }
        }

        public IEnumerable<string> SortCollection(IEnumerable<string> inputAsCollection, SortBy sortBy)
        {
            switch (sortBy)
            {
                case SortBy.Ascending:
                    return inputAsCollection.OrderBy(q => q);
                case SortBy.Descending:
                    return inputAsCollection.OrderByDescending(q => q);
                default:
                    return inputAsCollection;
            }
        }

        public IEnumerable<int> ConvertToIntCollection(IEnumerable<string> inputAsStringCollection)
        {
            try
            {
                return inputAsStringCollection.Select(q => Convert.ToInt32(q)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Fel: Kunde inte konvertera till en int-samling.");
                throw e;
            }
        }
    }
}
