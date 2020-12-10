namespace AdventOfCode2020.Domain.Day10
{
    internal class Adapter
    {
        public int Joltage { get; set; }
        public Adapter Previous { get; set; }
        public Adapter Next { get; set; }
        public int Differance => (IsPluggedIn) ? Joltage - Previous.Joltage : 0;
        public bool IsInUse => IsPluggedIn || HasPluggedIn;
        public bool IsPluggedIn => Previous != default;
        public bool HasPluggedIn => Next != default;

        public bool PlugInTo(Adapter plugInTo)
        {
            if (!plugInTo.HasPluggedIn && !IsPluggedIn)
            {
                Previous = plugInTo;
                plugInTo.Next = this;
                return true;
            }

            return false;
        }
    }
}
