using AdventOfCode2020.Tools;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            new ChallengeRoot(verbose: true)
                .Load(15, Part.Second)
                .RunAsync()
                .Wait();
        }
    }
}
