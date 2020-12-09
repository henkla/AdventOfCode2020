using AdventOfCode2020.Domain;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            new ChallengeFactory()
                .LoadAll()
                .Run();
        }
    }
}
