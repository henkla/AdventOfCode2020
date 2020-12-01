using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01.Library
{
    public class InputReader
    {
        public IEnumerable<int> GetInputCollection(string inputPath, bool orderByAscending = false)
        {
            // read input and store in array
            var inputAsAList = File.ReadAllLines(inputPath)
                .Select(int.Parse)
                .ToList();

            if (orderByAscending is true)
            {
                inputAsAList.Sort();
            }

            return inputAsAList;
        }
    }
}
