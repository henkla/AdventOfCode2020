using AdventOfCode2020.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day04
{
    class Program
    {
        /*
        --- Day 4: Passport Processing ---
        You arrive at the airport only to realize that you grabbed your 
        North Pole Credentials instead of your passport. While these documents 
        are extremely similar, North Pole Credentials aren't issued by a country 
        and therefore aren't actually valid documentation for travel in most 
        of the world.

        It seems like you're not the only one having problems, though; a very 
        long line has formed for the automatic passport scanners, and the delay 
        could upset your travel itinerary.

        Due to some questionable network security, you realize you might be 
        able to solve both of these problems at the same time.

        The automatic passport scanners are slow because they're having trouble 
        detecting which passports have all required fields. The expected fields 
        are as follows:

        byr (Birth Year)
        iyr (Issue Year)
        eyr (Expiration Year)
        hgt (Height)
        hcl (Hair Color)
        ecl (Eye Color)
        pid (Passport ID)
        cid (Country ID)
        Passport data is validated in batch files (your puzzle input). Each 
        passport is represented as a sequence of key:value pairs separated by 
        spaces or newlines. Passports are separated by blank lines.
        
        Here is an example batch file containing four passports:
        
        <example.txt>
         */
        static void Main(string[] args)
        {
            var inputHelper = new InputHelper();
            var input = inputHelper.GetInputAsLines("input.txt");

            Part1(input);
            Part2(input);
        }

        /*
        The first passport is valid - all eight fields are present. The second 
        passport is invalid - it is missing hgt (the Height field).

        The third passport is interesting; the only missing field is cid, so 
        it looks like data from North Pole Credentials, not a passport at all! 
        Surely, nobody would mind if you made the system temporarily ignore 
        missing cid fields. Treat this "passport" as valid.

        The fourth passport is missing two fields, cid and byr. Missing cid 
        is fine, but missing any other field is not, so this passport is invalid.

        According to the above rules, your improved system would report 2 
        valid passports.

        Count the number of valid passports - those that have all required fields. 
        Treat cid as optional. In your batch file, how many passports are valid?
         */
        private static void Part1(IEnumerable<string> input)
        {
            Console.WriteLine("Part 1:\n");

            var parsedPassports = ParseInput(input: input, strictValidation: false);

            Console.WriteLine($"There are {parsedPassports.Count(p => p.IsValid)} valid passwords in batch file.");
            Console.ReadKey();
        }

        private static IEnumerable<Passport> ParseInput(IEnumerable<string> input, bool strictValidation)
        {
            // part of the requirements
            var canBeEmpty = PassportEntry.cid;

            // this will be the first parsed passport
            var currentPassport = new Passport(canBeEmpty, strictValidation);
            
            // a list to hold all of the passwords
            var allPassports = new List<Passport>();

            /*
            hcl:#ae17e1 iyr:2013
            eyr:2024
            ecl:brn pid:760753108 byr:1931
            hgt:179cm 
             */

            // iterate each line of the input
            foreach (var line in input)
            {
                // it's the end of a passport - put the current
                // one in the list and start with a new one
                if (string.IsNullOrEmpty(line))
                {
                    allPassports.Add(currentPassport);
                    currentPassport = new Passport(canBeEmpty, strictValidation);
                }
                else
                {
                    // split each line on space
                    var keyValuePairs = line.Split(' ');

                    // iterate each line of key:value
                    foreach (var pair in keyValuePairs)
                    {
                        // split the key:value into string array
                        var splitPair = pair.Split(':');

                        // add key and value to the passport as an entry,
                        // but make sure to convert the key (string) into 
                        //the PassportEntry enum type (just a safety measurement)
                        currentPassport.AddEntry(ConvertStringToPassportEntry(splitPair[0]), splitPair[1]);
                    }
                }
            }

            // add the last passport to the list
            allPassports.Add(currentPassport);
            return allPassports;
        }

        private static PassportEntry ConvertStringToPassportEntry(string key)
        {
            // this will be the default enum value
            var passportEntry = PassportEntry.Invalid;

            try
            {
                // test if the given string key can be converted to the enum type
                passportEntry = (PassportEntry)Enum.Parse(typeof(PassportEntry), key);
            }
            catch (Exception)
            {
                // obviously it couldn't, so ignore it
                Console.WriteLine($"Fel: {key} är inte ett giltigt värde för {nameof(PassportEntry)}");
            }
            
            // populated or not, this will be the returned enum value for the key provided
            return passportEntry;
        }

        /*
        <description>
         */
        private static void Part2(IEnumerable<string> input)
        {
            Console.WriteLine("Part 2:\n");

            var parsedPassports = ParseInput(input: input, strictValidation: true);

            Console.WriteLine($"There are {parsedPassports.Count(p => p.IsValid)} valid passwords in batch file.");
            Console.ReadKey();
        }
    }
}
