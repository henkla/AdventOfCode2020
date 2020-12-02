using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01.Library
{
    public class InputHelper
    {
        public IEnumerable<string> GetInput(string inputPath)
        {
            try
            {
                return File.ReadAllLines(inputPath).ToList();
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