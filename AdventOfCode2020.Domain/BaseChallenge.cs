using AdventOfCode2020.Tools;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Domain
{
    public abstract class BaseChallenge : IChallenge
    {
        protected long _result;
        protected readonly string _day;
        protected readonly string _name;
        protected readonly InputHelper _inputHelper;

        protected BaseChallenge()
        {
            _result = 0;
            _day = GetType().Namespace.Split('.').Last();
            _name = GetType().Name;
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
                    PrintResult(1);
                    SolveSecond();
                    PrintResult(2);
                    break;
            }
        }

        protected abstract void Initialize();

        protected abstract void SolveFirst();

        protected abstract void SolveSecond();

        protected void PrintResult(int part)
        {
            Console.WriteLine($"{_name}/{_day} part {part}, result is {_result}.");
            Console.ReadKey();
        }
    }
}