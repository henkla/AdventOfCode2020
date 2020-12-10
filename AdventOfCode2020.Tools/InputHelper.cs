using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Tools
{
    public class InputHelper
    {
        private readonly string _workingDirectory;

        public InputHelper(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
        }

        public IEnumerable<string> GetInputAsLines(string inputPath)
        {
            inputPath = _workingDirectory + inputPath;

            try
            {
                return File.ReadAllLines(inputPath).ToList();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file could not be found. Check your path: {inputPath}");
                throw e;
            }
        }

        public char[][] GetInputAsGrid(string inputPath)
        {
            inputPath = _workingDirectory + inputPath;

            try
            {
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
    }
}
