using AdventOfCode2020.Library;
using System;
using System.Collections.Generic;
using System.IO;
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
            var inputHelper = new InputHelper(Directory.GetCurrentDirectory() + "\\");
            var input = inputHelper.GetInputAsLines("example.txt");
            var instructions = ParseInput(input);

            Part1(instructions);
            Part2(instructions);
        }

        private static IEnumerable<Instruction> ParseInput(IEnumerable<string> input)
        {
            // we need a list to hold all instructions
            var instructions = new List<Instruction>();

            // we want each instruction to have an index
            var index = 0;

            // iterate each line in input
            foreach (var line in input)
            {
                // separate operation from argument on space
                var instruction = line.Split(' ');
                instructions.Add(new Instruction
                {
                    Index = index++,
                    HasBeenRun = false,
                    Operation = instruction[0],
                    Argument = int.Parse(instruction[1]),
                });
            }

            // we want each instruction to dynamically point to the next and
            // previous one
            foreach (var i in instructions)
            {
                if (i.Next == null)
                    i.Next = instructions.ElementAtOrDefault(i.Index + 1);

                if (i.Previous == null)
                    i.Previous = instructions.ElementAtOrDefault(i.Index - 1);
            }

            // tie it all together by letting last node's next be first,
            // and vice versa
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
        private static void Part1(IEnumerable<Instruction> all)
        {
            Console.WriteLine("\nPart 1:\n");

            var current = all.First();
            var accumulator = RunInstructions(all, ref current);
            
            Console.WriteLine($"For part 1, the result is {accumulator}.");
            Console.ReadKey();
        }

        /*
        After some careful analysis, you believe that exactly one instruction 
        is corrupted.

        Somewhere in the program, either a jmp is supposed to be a nop, or a 
        nop is supposed to be a jmp. (No acc instructions were harmed in the 
        corruption of this boot code.)

        The program is supposed to terminate by attempting to execute an instruction 
        immediately after the last instruction in the file. By changing exactly one 
        jmp or nop, you can repair the boot code and make it terminate correctly.

        For example, consider the same program from above:

        nop +0
        acc +1
        jmp +4
        acc +3
        jmp -3
        acc -99
        acc +1
        jmp -4
        acc +6
        
        If you change the first instruction from nop +0 to jmp +0, it would create a 
        single-instruction infinite loop, never leaving that instruction. If you change 
        almost any of the jmp instructions, the program will still eventually find 
        another jmp instruction and loop forever.
        
        However, if you change the second-to-last instruction (from jmp -4 to nop -4), 
        the program terminates! The instructions are visited in this order:
        
        nop +0  | 1
        acc +1  | 2
        jmp +4  | 3
        acc +3  |
        jmp -3  |
        acc -99 |
        acc +1  | 4
        nop -4  | 5
        acc +6  | 6

        After the last instruction (acc +6), the program terminates by attempting to 
        run the instruction below the last instruction in the file. With this change, 
        after the program terminates, the accumulator contains the value 8 (acc +1, 
        acc +1, acc +6).
        
        Fix the program so that it terminates normally by changing exactly one jmp 
        (to nop) or nop (to jmp). What is the value of the accumulator after the program 
        terminates?
         */
        private static void Part2(IEnumerable<Instruction> all)
        {
            Console.WriteLine("\nPart 2:\n");

            // we need a place to store each switched instruction
            var switched = new Stack<Instruction>();

            // we need a starting node, but it can't be the first, 
            // since that will break the outer while loop. in fact,
            // it doesn't matter which one we start out with, since 
            // each outer while loop iteration will se current to 
            // first
            var current = all.Last();

            // just a place to store the accumulator value we are 
            // looking for
            var accumulator = 0;

            // as long as index differs from 0, we have not switched
            // the correct instruction operation. so, continue switching
            // one by one until we reach zero (or current == all.First())
            while (current.Index > 0)
            {
                // for each iteration, reset the HasBeenRun flag
                all.ToList().ForEach(i => i.HasBeenRun = false);

                // make sure to start each instruction run with the
                // first instruction node
                current = all.First();

                // run the actual instructions and store the accumulator
                // value
                accumulator = RunInstructions(all, ref current);

                // if there are switched instructions, now is the time
                // to switch them back, and to switch the next instruction
                // node in the list
                if (switched.Any())
                {
                    // get the instruction that was switched last time
                    var currentlySwitched = switched.Pop();

                    // switch it back
                    currentlySwitched.SwitchOperation();

                    // switch the next node
                    currentlySwitched.Next.SwitchOperation();

                    // push the switched (next) node
                    switched.Push(currentlySwitched.Next);
                }
                else
                {
                    // this is the first run, so stack is empty
                    // pick the first node and switch it
                    var toBeSwitched = all.First();
                    toBeSwitched.SwitchOperation();
                    
                    // push the switched node to the stack so that we can
                    // switch it back the next iteration
                    switched.Push(toBeSwitched);
                }
            }

            Console.WriteLine($"For part 2, the result is {accumulator}.");
            Console.ReadKey();
        }

        private static int RunInstructions(IEnumerable<Instruction> all, ref Instruction current)
        {
            // we need somewhere to store the accumulator value (which will)
            // be the actual result
            var accumulator = 0;

            // continue for as long as we've stumbled across an 
            // instruction that was previously run
            while (!current.HasBeenRun)
            {
                // mark current instruction as completed
                current.HasBeenRun = true;

                // check the actual instruction
                switch (current.Operation)
                {
                    // just add the argument to the accumulator value and
                    // set current to the next instruction node
                    case "acc":
                        accumulator += current.Argument;
                        current = current.Next;
                        break;
                    // make a jump relative to self - the dynamic links should
                    // be circular so if we go out of boundaries, return from beginning
                    case "jmp":
                        var newPosition = (current.Index + current.Argument >= all.Count())
                            ? all.Count() % (current.Index + current.Argument)
                            : current.Index + current.Argument;
                        current = all.ElementAt(newPosition);
                        break;
                    // no operation - just set current to the next node
                    case "nop":
                    default:
                        current = current.Next;
                        break;
                }
            }

            // by now, we've reached our goal, which
            // is to find out the total accumulator value based
            // on the instructions given
            return accumulator;
        }
    }
}
