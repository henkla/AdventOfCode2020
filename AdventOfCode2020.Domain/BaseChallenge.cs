using AdventOfCode2020.Tools;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Domain
{
    public abstract class BaseChallenge : IChallenge
    {
        protected InputReader InputReader { get; private set; }
        public Result Result { get; private set; }
        public string Name => GetType().Name;
        public int Day { get; private set; }

        protected BaseChallenge()
        {
            Day = int.Parse(Regex.Match(GetType().Namespace.Substring(GetType().Namespace.Length - 2, 2), @"\d+").Value);
            Result = new Result(Name, Day);
            InputReader = new InputReader($"{Directory.GetCurrentDirectory()}\\Day{Day:D2}\\");
        }

        public void Run(Part part)
        {
            Initialize();

            switch (part)
            {
                case Part.First:
                    SolveFirst();
                    break;
                case Part.Second:
                    SolveSecond();
                    break;
                case Part.Both:
                    SolveFirst();
                    SolveSecond();
                    break;
            }

            Result.Print();
            Console.ReadKey();
        }

        protected abstract void Initialize();

        protected abstract void SolveFirst();

        protected abstract void SolveSecond();
    }
}