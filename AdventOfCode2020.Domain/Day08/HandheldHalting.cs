using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day08
{
    public class HandheldHalting : BaseChallenge
    {
        private IEnumerable<string> _input;
        private IEnumerable<Instruction> _instructions;

        protected override void Initialize()
        {
            _input = _inputHelper.GetInputAsLines("input.txt");
            _instructions = ParseInput(_input);
        }

        protected override void SolveFirst()
        {
            var current = _instructions.First();
            var accumulator = RunInstructions(_instructions, ref current);

            _result = accumulator;
        }

        protected override void SolveSecond()
        {
            // reset the instructions
            _instructions = ParseInput(_input);

            // we need a place to store each switched instruction
            var switched = new Stack<Instruction>();

            // we need a starting node, but it can't be the first, 
            // since that will break the outer while loop. in fact,
            // it doesn't matter which one we start out with, since 
            // each outer while loop iteration will se current to 
            // first
            var current = _instructions.Last();

            // just a place to store the accumulator value we are 
            // looking for
            var accumulator = 0;

            // as long as index differs from 0, we have not switched
            // the correct instruction operation. so, continue switching
            // one by one until we reach zero (or current == all.First())
            while (current.Index > 0)
            {
                // for each iteration, reset the HasBeenRun flag
                _instructions.ToList().ForEach(i => i.HasBeenRun = false);

                // make sure to start each instruction run with the
                // first instruction node
                current = _instructions.First();

                // run the actual instructions and store the accumulator
                // value
                accumulator = RunInstructions(_instructions, ref current);

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
                    var toBeSwitched = _instructions.First();
                    toBeSwitched.SwitchOperation();

                    // push the switched node to the stack so that we can
                    // switch it back the next iteration
                    switched.Push(toBeSwitched);
                }
            }

            _result = accumulator;
        }

        private IEnumerable<Instruction> ParseInput(IEnumerable<string> input)
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

        private int RunInstructions(IEnumerable<Instruction> all, ref Instruction current)
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
