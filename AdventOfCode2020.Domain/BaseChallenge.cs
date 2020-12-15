using AdventOfCode2020.Tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain
{
    public abstract class BaseChallenge : IChallenge
    {
        private Converter _converter;

        protected Converter Converter
        {
            get
            {
                if (_converter == null)
                    _converter = new Converter();

                return _converter;
            }
        }
        protected InputReader InputReader { get; private set; }
        protected Stopwatch Stopwatch { get; private set; }
        public string Name => GetType().Name;
        public int Day { get; private set; }
        public Result Result { get; private set; }

        protected BaseChallenge()
        {
            Day = GetType().Namespace.Split('.').Last().ExtractNumbers();
            InputReader = new InputReader($"{Directory.GetCurrentDirectory()}\\Day{Day:D2}\\");
            Stopwatch = new Stopwatch();
            Result = new Result(Name, Day);
        }

        public async Task RunAsync(Part part)
        {
            Initialize();

            Console.WriteLine($"\nRunning {Day:D2} / {Name}...");

            switch (part)
            {
                case Part.First:
                    Result.Add(await RunAsync(SolveFirst));
                    break;
                case Part.Second:
                    Result.Add(await RunAsync(SolveSecond));
                    break;
                case Part.Both:
                    Result.Add(await RunAsync(SolveFirst));
                    Result.Add(await RunAsync(SolveSecond));
                    break;
            }

            Result.Print();
        }

        private async Task<PartialResult> RunAsync(Func<PartialResult> partToRun)
        {
            Stopwatch.Restart();
            return await Task.Run(() => partToRun.Invoke());
        }

        protected abstract void Initialize();

        protected abstract PartialResult SolveFirst();

        protected abstract PartialResult SolveSecond();
    }
}