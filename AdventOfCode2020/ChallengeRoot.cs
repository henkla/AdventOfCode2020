﻿using AdventOfCode2020.Domain;
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
using AdventOfCode2020.Domain.Day11;
using AdventOfCode2020.Domain.Day12;
using AdventOfCode2020.Domain.Day13;
using AdventOfCode2020.Domain.Day14;
using AdventOfCode2020.Domain.Day15;
using AdventOfCode2020.Domain.Day16;
using AdventOfCode2020.Domain.Day17;
using AdventOfCode2020.Domain.Day18;
using AdventOfCode2020.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    internal class ChallengeRoot
    {
        private readonly bool _verbose;
        private readonly IDictionary<string, (IChallenge Challenge, Part Part)> _challenges;
        private readonly uint _maxDays;
        private readonly uint _day;

        public ChallengeRoot(bool verbose = false)
        {
            _verbose = verbose;
            _challenges = new Dictionary<string, (IChallenge Challenge, Part Part)>();
            _maxDays = 25;
            _day = 18;
        }

        public IEnumerable<IChallenge> GetLoadedChallenges()
        {
            return _challenges.Values.Select(v => v.Challenge);
        }

        public ChallengeRoot Load(Challenge challenge, Part part = Part.Both)
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

        public ChallengeRoot Load(uint day, Part part = Part.Both)
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
                    AddOrUpdateChallenge<SeatingSystem>(part);
                    break;
                case 12:
                    AddOrUpdateChallenge<RainRisk>(part);
                    break;
                case 13:
                    AddOrUpdateChallenge<ShuttleSearch>(part);
                    break;
                case 14:
                    AddOrUpdateChallenge<DockingData>(part);
                    break;
                case 15:
                    AddOrUpdateChallenge<RambunctiousRecitation>(part);
                    break;
                case 16:
                    AddOrUpdateChallenge<TicketTranslation>(part);
                    break;
                case 17:
                    AddOrUpdateChallenge<ConwayCubes>(part);
                    break;
                case 18:
                    AddOrUpdateChallenge<OperationOrder>(part);
                    break;
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                default:
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

            if (_verbose)
            {
                Console.WriteLine($"Loaded {_challenges[key].Challenge.Day:D2} / {key}, {(part == Part.Both ? "both parts" : $"{part.ToString().ToLower()} part")}...");
            }
        }

        public async Task RunAsync()
        {
            foreach (var challenge in _challenges.OrderBy(c => c.Value.Challenge.Day))
            {
                try
                {
                    await challenge.Value.Challenge.RunAsync(challenge.Value.Part);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ajabaja: {e.Message}");
                }
            }

            if (_verbose)
            {
                Console.WriteLine($"\n{(_challenges.Count() == 1 ? "Loaded challenge" : $"All {_challenges.Count()} loaded challenges")} finished! Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
