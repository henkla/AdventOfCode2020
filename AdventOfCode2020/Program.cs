﻿using AdventOfCode2020.Tools;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main()
        {
            new ChallengeRoot()
                .Load(Challenge.Latest, Part.Both)
                .RunAsync()
                .Wait();
        }
    }
}
