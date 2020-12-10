using AdventOfCode2020.Tools;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Domain
{
    public abstract class BaseChallenge : IChallenge
    {
        protected readonly InputHelper _inputHelper;
        protected readonly string _day;
        protected Result _result { get; private set; }

        protected BaseChallenge()
        {
            _day = GetType().Namespace.Split('.').Last();
            _result = new Result(GetType().Name, int.Parse(_day.Substring(_day.Length - 2, 2)));
            _inputHelper = new InputHelper(Directory.GetCurrentDirectory() + "\\" + _day + "\\");
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

            PrintResult();
        }

        protected abstract void Initialize();

        protected abstract void SolveFirst();

        protected abstract void SolveSecond();

        protected void PrintResult()
        {
            _result.Print();
        }
    }
}