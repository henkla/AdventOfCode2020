using AdventOfCode2020.Tools;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            new ChallengeRoot(verbose: true)
                .Load(Challenge.All, Part.Both)
                .Load(Challenge.Latest, Part.First)
                .Run();
        }
    }
}
