using AdventOfCode2020.Domain;
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
using AdventOfCode2020.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    internal class ChallengeFactory
    {
        private IDictionary<string, (IChallenge Challenge, Part Part)> _challenges;
        private readonly uint _maxDays;
        private readonly uint _day;

        public ChallengeFactory()
        {
            _challenges = new Dictionary<string, (IChallenge Challenge, Part Part)>();
            _maxDays = 25;
            _day = 10;
        }

        public IEnumerable<IChallenge> GetLoadedChallenges()
        {
            return _challenges.Values.Select(v => v.Challenge);
        }

        public ChallengeFactory Load(Challenge challenge, Part part = Part.Both)
        {
            switch (challenge)
            {
                case Challenge.All:
                    for (uint d = 1; d <= _day; d++)
                    {
                        Load(d, part);
                    }
                    break;
                case Challenge.Latest:
                    Load(_day, part);
                    break;
            }

            return this;
        }

        public ChallengeFactory Load(uint day, Part part = Part.Both)
        {
            if (day > _maxDays)
                throw new ArgumentException($"There are only a maximum of {_maxDays} challenges - value {day} is not valid.");

            switch (day)
            {
                case 1:
                    AddOrUpdateChallenge<ReportRepair>(part);
                    break;
                case 2:
                    AddOrUpdateChallenge<PasswordPhilosophy>(part);
                    break;
                case 3:
                    AddOrUpdateChallenge<TobogganTrajectory>(part);
                    break;
                case 4:
                    AddOrUpdateChallenge<PassportProcessing>(part);
                    break;
                case 5:
                    AddOrUpdateChallenge<BinaryBoarding>(part);
                    break;
                case 6:
                    AddOrUpdateChallenge<CustomCustoms>(part);
                    break;
                case 7:
                    AddOrUpdateChallenge<HandyHaversacks>(part);
                    break;
                case 8:
                    AddOrUpdateChallenge<HandheldHalting>(part);
                    break;
                case 9:
                    AddOrUpdateChallenge<EncodingError>(part);
                    break;
                case 10:
                    AddOrUpdateChallenge<AdapterArray>(part);
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    throw new NotImplementedException($"The challenge for day {day} has not been created yet.");
            }

            return this;
        }

        private void AddOrUpdateChallenge<T>(Part part) where T : IChallenge
        {
            var key = typeof(T).Name;
            if (_challenges.ContainsKey(key))
            {
                _challenges[key] = (_challenges[key].Challenge, part);
            }
            else
            {
                _challenges.Add(key, (Activator.CreateInstance<T>(), part));
            }
        }

        public void Run()
        {
            foreach (var challenge in _challenges)
            {
                challenge.Value.Challenge.Run(challenge.Value.Part);
            }
        }
    }
}
