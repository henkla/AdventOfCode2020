using AdventOfCode2020.Domain.Day01;
using AdventOfCode2020.Domain.Day02;
using AdventOfCode2020.Domain.Day03;
using AdventOfCode2020.Domain.Day04;
using AdventOfCode2020.Domain.Day05;
using AdventOfCode2020.Domain.Day06;
using AdventOfCode2020.Domain.Day07;
using AdventOfCode2020.Domain.Day08;
using AdventOfCode2020.Domain.Day09;
using AdventOfCode2020.Domain.Day10;
using System;
using System.Collections.Generic;

namespace AdventOfCode2020.Domain
{
    public enum Part
    {
        Both = 0,
        First = 1,
        Second = 2
    }

    public class ChallengeFactory
    {
        private IDictionary<IChallenge, Part> _challenges;
        private const int _day = 10;
        private const int _maxDays = 25;

        public ChallengeFactory()
        {
            _challenges = new Dictionary<IChallenge, Part>();
        }

        public IEnumerable<IChallenge> GetChallenges()
        {
            return _challenges.Keys;
        }

        public ChallengeFactory LoadAll(Part part = Part.Both)
        {
            for (uint d = 1; d <= _day; d++)
            {
                LoadDay(d, part);
            }

            return this;
        }

        public ChallengeFactory LoadDay(uint day, Part part = Part.Both)
        {
            if (day > _maxDays)
                throw new ArgumentException($"There are only a maximum of {_maxDays} challenges - submitted value {day} is not valid.");

            switch (day)
            {
                case 1:
                    _challenges.Add(new ReportRepair(), part);
                    break;
                case 2:
                    _challenges.Add(new PasswordPhilosophy(), part);
                    break;
                case 3:
                    _challenges.Add(new TobogganTrajectory(), part);
                    break;
                case 4:
                    _challenges.Add(new PassportProcessing(), part);
                    break;
                case 5:
                    _challenges.Add(new BinaryBoarding(), part);
                    break;
                case 6:
                    _challenges.Add(new CustomCustoms(), part);
                    break;
                case 7:
                    _challenges.Add(new HandyHaversacks(), part);
                    break;
                case 8:
                    _challenges.Add(new HandheldHalting(), part);
                    break;
                case 9:
                    _challenges.Add(new EncodingError(), part);
                    break;
                case 10:
                    _challenges.Add(new AdapterArray(), part);
                    break;
                default:
                    throw new NotImplementedException($"The challenge for day {day} has not been created yet.");
            }

            return this;
        }

        public ChallengeFactory LoadLatest(Part part = Part.Both)
        {
            return LoadDay(_day, part);
        }

        public void Run()
        {
            foreach (var challenge in _challenges)
            {
                switch (challenge.Value)
                {
                    case Part.Both:
                        challenge.Key.RunBoth();
                        break;
                    case Part.First:
                        challenge.Key.RunFirst();
                        break;
                    case Part.Second:
                        challenge.Key.RunSecond();
                        break;
                }
            }
        }
    }
}
