using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Domain.Day04
{
    public enum PassportEntry
    {
        Invalid = 0,
        byr = 1,
        iyr = 2,
        eyr = 3,
        hgt = 4,
        hcl = 5,
        ecl = 6,
        pid = 7,
        cid = 8
    }

    public class Passport
    {
        private readonly IDictionary<PassportEntry, string> _entries;
        private readonly PassportEntry _canBeEmpty;
        private readonly bool _strictValidation;

        public string BirthYear { get { return _entries[PassportEntry.byr]; } }
        public string IssueYear { get { return _entries[PassportEntry.iyr]; } }
        public string ExpirationYear { get { return _entries[PassportEntry.eyr]; } }
        public string Height { get { return _entries[PassportEntry.hgt]; } }
        public string HairColor { get { return _entries[PassportEntry.hcl]; } }
        public string EyeColor { get { return _entries[PassportEntry.ecl]; } }
        public string ID { get { return _entries[PassportEntry.pid]; } }
        public string CountryID { get { return _entries[PassportEntry.cid]; } }
        public bool IsValid => CheckIfPassportIsValid();

        public Passport(PassportEntry canBeEmpty, bool strictValidation = false)
        {
            _canBeEmpty = canBeEmpty;
            _entries = new Dictionary<PassportEntry, string>();
            _strictValidation = strictValidation;

            foreach (PassportEntry entry in Enum.GetValues(typeof(PassportEntry)))
            {
                if (entry != PassportEntry.Invalid)
                {
                    AddEntry(entry, string.Empty);
                }
            }
        }

        public void AddEntry(PassportEntry key, string value)
        {
            // if for some reason a key is invalid, ignore it
            if (key != PassportEntry.Invalid)
            {
                // check strictness
                if (_strictValidation)
                {
                    _entries[key] = GetKeyValueIfValid(key, value);
                }
                else
                {
                    _entries[key] = value;
                }
            }
        }

        private string GetKeyValueIfValid(PassportEntry key, string value)
        {
            // the value to be returned
            var validatedValue = string.Empty;

            // clean out empty values - no need to check them
            if (string.IsNullOrEmpty(value))
            {
                return validatedValue;
            }

            switch (key)
            {
                // byr (Birth Year) - four digits; at least 1920 and at most 2002.
                case PassportEntry.byr:

                    if (int.TryParse(value, out int numericValue)
                        && numericValue >= 1920
                        && numericValue <= 2002)
                    {
                        validatedValue = value;
                    }
                    break;

                // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                case PassportEntry.iyr:

                    if (int.TryParse(value, out numericValue)
                        && numericValue >= 2010
                        && numericValue <= 2020)
                    {
                        validatedValue = value;
                    }
                    break;

                // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                case PassportEntry.eyr:

                    if (int.TryParse(value, out numericValue)
                        && numericValue >= 2020
                        && numericValue <= 2030)
                    {
                        validatedValue = value;
                    }
                    break;

                // hgt (Height) - a number followed by either cm or in:
                case PassportEntry.hgt:

                    if (value.Contains("cm"))
                    {
                        // If cm, the number must be at least 150 and at most 193.
                        if (int.TryParse(Regex.Match(value, @"\d+").Value, out numericValue) && numericValue >= 150 && numericValue <= 193)
                        {
                            validatedValue = value;
                        }
                    }
                    else if (value.Contains("in"))
                    {
                        // If in, the number must be at least 59 and at most 76.
                        if (int.TryParse(Regex.Match(value, @"\d+").Value, out numericValue) && numericValue >= 59 && numericValue <= 76)
                        {
                            validatedValue = value;
                        }
                    }
                    break;

                // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                case PassportEntry.hcl:

                    if (value.First() == '#' && value.Length == (1 + 6))
                    {
                        if (int.TryParse(value.Substring(1),
                                         NumberStyles.HexNumber,
                                         CultureInfo.CurrentCulture,
                                         out _))
                        {
                            validatedValue = value;
                        }
                    }
                    break;

                // ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                case PassportEntry.ecl:

                    if (string.Equals(value, "amb")
                      ^ string.Equals(value, "blu")
                      ^ string.Equals(value, "brn")
                      ^ string.Equals(value, "gry")
                      ^ string.Equals(value, "grn")
                      ^ string.Equals(value, "hzl")
                      ^ string.Equals(value, "oth"))
                    {
                        validatedValue = value;
                    }
                    break;

                // pid (Passport ID) - a nine-digit number, including leading zeroes.
                case PassportEntry.pid:

                    if (value.Length == 9 && int.TryParse(value, out _))
                    {
                        validatedValue = value;
                    }
                    break;

                // cid (Country ID) - ignored, missing or not.
                case PassportEntry.cid:
                case PassportEntry.Invalid:
                default:
                    break;
            }

            // return variable - populated or not
            return validatedValue;
        }

        private bool CheckIfPassportIsValid()
        {
            return !_entries.Where(e => string.IsNullOrEmpty(e.Value))
                .Except(_entries.Where(e => e.Key == _canBeEmpty))
                .Any();
        }
    }
}
