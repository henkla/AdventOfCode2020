namespace Day08
{
    public class Instruction
    {
        public int Index { get; set; }
        public string Operation { get; set; }
        public int Argument { get; set; }
        public bool HasBeenRun { get; set; }
        public Instruction Previous { get; set; }
        public Instruction Next { get; set; }
    }
}
