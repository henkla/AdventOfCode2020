using AdventOfCode2020.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day08
{
    class Program
    {
        /*
        Your flight to the major airline hub reaches cruising altitude without 
        incident. While you consider checking the in-flight menu for one of those 
        drinks that come with a little umbrella, you are interrupted by the kid 
        sitting next to you.

        Their handheld game console won't turn on! They ask if you can take a look.
        
        You narrow the problem down to a strange infinite loop in the boot code 
        (your puzzle input) of the device. You should be able to fix it, but 
        first you need to be able to run the code in isolation.
        
        The boot code is represented as a text file with one instruction per line 
        of text. Each instruction consists of an operation (acc, jmp, or nop) and 
        an argument (a signed number like +4 or -20).
        
        - acc increases or decreases a single global value called the accumulator 
          by the value given in the argument. For example, acc +7 would increase 
          the accumulator by 7. The accumulator starts at 0. After an acc instruction, 
          the instruction immediately below it is executed next.
        - jmp jumps to a new instruction relative to itself. The next instruction 
          to execute is found using the argument as an offset from the jmp instruction; 
          for example, jmp +2 would skip the next instruction, jmp +1 would continue 
          to the instruction immediately below it, and jmp -20 would cause the 
          instruction 20 lines above to be executed next.
        - nop stands for No OPeration - it does nothing. The instruction immediately 
          below it is executed next.

        For example, consider the following program:
        
        nop +0
        acc +1
        jmp +4
        acc +3
        jmp -3
        acc -99
        acc +1
        jmp -4
        acc +6
        
        These instructions are visited in this order:
        
        nop +0  | 1
        acc +1  | 2, 8(!)
        jmp +4  | 3
        acc +3  | 6
        jmp -3  | 7
        acc -99 |
        acc +1  | 4
        jmp -4  | 5
        acc +6  |
        
        First, the nop +0 does nothing. Then, the accumulator is increased 
        from 0 to 1 (acc +1) and jmp +4 sets the next instruction to the 
        other acc +1 near the bottom. After it increases the accumulator from 
        1 to 2, jmp -4 executes, setting the next instruction to the only acc 
        +3. It sets the accumulator to 5, and jmp -3 causes the program to 
        continue back at the first acc +1.
         */
        static void Main(string[] args)
        {
            var inputHelper = new InputHelper();
            var input = inputHelper.GetInputAsLines("input.txt");

            var instructions = ParseInput(input);

            Part1(instructions);
            Part2(instructions);
        }

        private static IEnumerable<Instruction> ParseInput(IEnumerable<string> input)
        {
            var instructions = new List<Instruction>();

            var index = 0;
            foreach (var line in input)
            {
                var instruction = line.Split(' ');
                instructions.Add(new Instruction
                {
                    Index = index++,
                    HasBeenRun = false,
                    Operation = instruction[0],
                    Argument = int.Parse(instruction[1]),
                });
            }

            foreach (var i in instructions)
            {
                if (i.Next == null)
                    i.Next = instructions.ElementAtOrDefault(i.Index + 1);

                if (i.Previous == null)
                    i.Previous = instructions.ElementAtOrDefault(i.Index - 1);
            }

            // tie it all together
            instructions.First().Previous = instructions.Last();
            instructions.Last().Next = instructions.Last();

            return instructions;
        }

        /*
        This is an infinite loop: with this sequence of jumps, the program 
        will run forever. The moment the program tries to run any instruction 
        a second time, you know it will never terminate.

        Immediately before the program would run an instruction a second time, 
        the value in the accumulator is 5.

        Run your copy of the boot code. Immediately before any instruction is 
        executed a second time, what value is in the accumulator?
         */
        private static void Part1(IEnumerable<Instruction> instructions)
        {
            Console.WriteLine("\nPart 1:\n");

            var accumulator = 0;
            var currentInstruction = instructions.First();
            while (!currentInstruction.HasBeenRun)
            {
                currentInstruction.HasBeenRun = true;
                switch (currentInstruction.Operation)
                {
                    case "acc":
                        accumulator += currentInstruction.Argument;
                        currentInstruction = currentInstruction.Next;
                        break;
                    case "jmp":
                        var newPosition = GetNewPosition(currentInstruction, instructions.Count());
                        currentInstruction = instructions.ElementAt(newPosition);
                        break;
                    case "nop":
                    default:
                        currentInstruction = currentInstruction.Next;
                        break;
                }
            }

            Console.WriteLine($"For part 1, the result is {accumulator}.");
            Console.ReadKey();
        }

        /*
         <description>
         */
        private static void Part2(IEnumerable<Instruction> instructions)
        {
            throw new NotImplementedException();
            Console.WriteLine("\nPart 2:\n");

            // do stuff

            Console.WriteLine($"For part 2, the result is RESULT.");
            Console.ReadKey();
        }

        private static int GetNewPosition(Instruction current, int total)
        {
            return (current.Index + current.Argument > total)
                ? total % (current.Index + current.Argument)
                : current.Index + current.Argument;
        }
    }
}
