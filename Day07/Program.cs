﻿using AdventOfCode2020.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day07
{
    class Program
    {
        /*
        You land at the regional airport in time for your next flight. In fact, 
        it looks like you'll even have time to grab some food: all flights are 
        currently delayed due to issues in luggage processing.

        Due to recent aviation regulations, many rules (your puzzle input) are 
        being enforced about bags and their contents; bags must be color-coded 
        and must contain specific quantities of other color-coded bags. Apparently, 
        nobody responsible for these regulations considered how long they would 
        take to enforce!
        
        For example, consider the following rules:
        
        light red bags contain 1 bright white bag, 2 muted yellow bags.
        dark orange bags contain 3 bright white bags, 4 muted yellow bags.
        bright white bags contain 1 shiny gold bag.
        muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
        shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
        dark olive bags contain 3 faded blue bags, 4 dotted black bags.
        vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
        faded blue bags contain no other bags.
        dotted black bags contain no other bags.
        
        These rules specify the required contents for 9 bag types. In this example, 
        every faded blue bag is empty, every vibrant plum bag contains 11 bags 
        (5 faded blue and 6 dotted black), and so on.
        
        You have a shiny gold bag. If you wanted to carry it in at least one 
        other bag, how many different bag colors would be valid for the outermost 
        bag? (In other words: how many colors can, eventually, contain at least 
        one shiny gold bag?)
        
        In the above rules, the following options would be available to you:
        
        A bright white bag, which can hold your shiny gold bag directly.
        A muted yellow bag, which can hold your shiny gold bag directly, plus 
        some other bags. A dark orange bag, which can hold bright white and 
            muted yellow bags, either of which could then hold your shiny gold bag.
        A light red bag, which can hold bright white and muted yellow bags, 
            either of which could then hold your shiny gold bag.
        So, in this example, the number of bag colors that can eventually contain 
            at least one shiny gold bag is 4.
         */
        static void Main(string[] args)
        {
            var inputHelper = new InputHelper();
            var input = inputHelper.GetInputAsLines("input.txt");

            // operate on input?

            Part1(input);
            //Part2(input);
        }

        /*
        How many bag colors can eventually contain at least one shiny gold bag? 
        (The list of rules is quite long; make sure you get all of it.)
         */
        private static void Part1(IEnumerable<string> input)
        {
            Console.WriteLine("\nPart 1:\n");

            var parsedRules = new List<Rule>();
            foreach (var line in input)
            {
                var ls = line.Split(" bags contain");
                parsedRules.Add(new Rule(ls[0], ls[1]));
            }

            foreach (var rule in parsedRules)
            {
                if (rule.Contents.Contains("no")) 
                    continue;

                var splitContents = rule.Contents.Split(", ");
                foreach (var content in splitContents)
                {
                    var parsedContent = Regex.Replace(content.Trim().Split(" bag")[0], @"[\d-]", string.Empty).Trim();
                    var contentRule = parsedRules.Single(r => string.Equals(r.Type, parsedContent));
                    
                    contentRule.Parents.Add(rule);
                    rule.Children.Add(contentRule);
                    
                    rule.Contents = string.Empty;
                }
            }

            var theContainers = new HashSet<string>();
            var theRule = parsedRules.Single(r => string.Equals(r.Type, "shiny gold"));
            ExtractParents(ref theContainers, theRule);
            
            Console.WriteLine($"For part 1, the result is {theContainers.Count}.");
            Console.ReadKey();
        }

        private static void ExtractParents(ref HashSet<string> theContainers, Rule rule)
        {
            foreach (var r in rule.Parents)
            {
                theContainers.Add(r.Type);
                ExtractParents(ref theContainers, r);
            }
        }

        /*
        <description> 
         */
        private static void Part2(IEnumerable<string> input)
        {
            throw new NotImplementedException();
            Console.WriteLine("\nPart 2:\n");

            // do stuff

            Console.WriteLine($"For part 2, the result is RESULT.");
            Console.ReadKey();
        }
    }
}
