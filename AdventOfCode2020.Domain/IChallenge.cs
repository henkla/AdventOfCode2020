using AdventOfCode2020.Tools;
using System.Threading.Tasks;

namespace AdventOfCode2020.Domain
{
    public interface IChallenge
    {
        Result Result { get; }
        int Day { get; }
        string Name { get; }
        Task RunAsync(Part part);
    }
}
