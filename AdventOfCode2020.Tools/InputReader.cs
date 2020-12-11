using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Tools
{
    public class InputReader
    {
        private readonly string _workingDirectory;

        public InputReader(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
        }

        public IEnumerable<string> ReadFile(string input)
        {
            input = input.Insert(0, _workingDirectory);

            try
            {
                return File.ReadAllLines(input).ToList();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file could not be found. Check your path:\n{input}");
                throw e;
            }
        }
    }
}
