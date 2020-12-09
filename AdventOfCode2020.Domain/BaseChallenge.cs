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

        public void RunBoth()
        {
            Initialize();
            SolveFirst();
            PrintResult(1);
            SolveSecond();
            PrintResult(2);
        }

        public void RunFirst()
        {
            Initialize();
            SolveFirst();
            PrintResult(1);
        }

        public void RunSecond()
        {
            Initialize();
            SolveSecond();
            PrintResult(2);
        }

        protected abstract void Initialize();

        protected abstract void SolveFirst();

        protected abstract void SolveSecond();

        protected void PrintResult(int part)
        {
            Console.WriteLine($"{_name} ({_day}) part {part}, result is {_result}.");
            Console.ReadKey();
        }
    }
}