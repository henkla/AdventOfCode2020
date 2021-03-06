﻿namespace AdventOfCode2020.Domain.Day08
{
    internal class Instruction
    {
        public int Index { get; set; }
        public string Operation { get; set; }
        public int Argument { get; set; }
        public bool HasBeenRun { get; set; }
        public Instruction Previous { get; set; }
        public Instruction Next { get; set; }

        public void SwitchOperation()
        {
            if (Operation == "jmp")
                Operation = "nop";
            else if (Operation == "nop")
                Operation = "jmp";
        }
    }
}
