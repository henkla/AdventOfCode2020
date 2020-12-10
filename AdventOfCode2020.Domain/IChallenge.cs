using AdventOfCode2020.Tools;

namespace AdventOfCode2020.Domain
{
    public interface IChallenge
    {
        Result Result { get; }
        int Day { get; }
        string Name { get; }
        void Run(Part part);
    }
}
