using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day04
{
    public class PassportProcessing : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = _inputHelper.GetInputAsLines("input.txt");
        }

        protected override void SolveFirst()
        {
            var parsedPassports = ParseInput(_input, strictValidation: false);
            _result.First = parsedPassports.Count(p => p.IsValid);
        }

        protected override void SolveSecond()
        {
            var parsedPassports = ParseInput(_input, strictValidation: true);
            _result.Second = parsedPassports.Count(p => p.IsValid);
        }

        private IEnumerable<Passport> ParseInput(IEnumerable<string> input, bool strictValidation)
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

        private PassportEntry ConvertStringToPassportEntry(string key)
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
    }
}
