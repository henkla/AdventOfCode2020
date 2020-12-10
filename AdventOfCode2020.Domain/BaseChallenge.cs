using AdventOfCode2020.Tools;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Domain
{
    internal abstract class BaseChallenge : IChallenge
    {
        protected readonly InputHelper _inputHelper;
        protected readonly string _day;
        protected readonly string _name;
        protected Result _result { get; private set; }

        protected BaseChallenge()
        {
            _name = GetType().Name;
            _day = GetType().Namespace.Split('.').Last();
            _result = new Result();
            _inputHelper = new InputHelper(Directory.GetCurrentDirectory() + "\\" + _day + "\\");
        }

        public void Run(Part part)
        {
            Initialize();

            switch (part)
            {
                case Part.First:
                    SolveFirst();
                    PrintResult(1);
                    break;
                case Part.Second:
                    SolveSecond();
                    PrintResult(2);
                    break;
                case Part.Both:
                    SolveFirst();
                    SolveSecond();
                    PrintResult(1);
                    PrintResult(2);
                    break;
            }
        }

        protected abstract void Initialize();

        protected abstract void SolveFirst();

        protected abstract void SolveSecond();

        protected void PrintResult(uint part)
        {
            Console.WriteLine($"{_name}/{_day} part {part}, result is {_result.GetPart(part)}.");
            Console.ReadKey();
        }
    }
}