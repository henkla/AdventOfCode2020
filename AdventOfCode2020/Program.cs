﻿using AdventOfCode2020.Tools;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            new ChallengeRoot()
                .Load(Challenge.Latest, Part.First)
                .Run();
        }
    }
}
