using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day16
{
    public class TicketTranslation : BaseChallenge
    {
        protected override void Initialize()
        {
            var _input = InputReader.ReadFile("example.txt");
        }

        protected override PartialResult SolveFirst()
        {
            return new PartialResult(1, 0, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            return new PartialResult(2, 0, Stopwatch.Elapsed);
        }
    }
}
