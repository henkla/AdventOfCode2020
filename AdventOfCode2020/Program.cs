using AdventOfCode2020.Tools;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            new ChallengeFactory()
                .Load(Challenge.All, Part.Both)
                .Run();
        }
    }
}
