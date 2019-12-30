using Blacksmith.Extensions.Randoms;
using System;

namespace Blacksmith.Extensions.RandomNumbers
{
    public static class RandomNumberExtensions
    {
        private static Random currentRandom;

        static RandomNumberExtensions()
        {
            int seed;

            seed = prv_generateSeed();
            currentRandom = new Random(seed);
        }

        public static Random CurrentRandom
        {
            get => currentRandom;
            set => currentRandom = value ?? throw new ArgumentNullException(nameof(CurrentRandom));
        }

        public static bool at(this bool value, double percentage, Random random = null)
        {
            bool mustReturnValue;

            if (percentage <= 0 || 100 <= percentage)
                throw new ArgumentOutOfRangeException(nameof(percentage), $"The {nameof(percentage)} parameter must be between 0.0 and 100.0 excluding both.");

            random = random ?? currentRandom;
            mustReturnValue = random.NextDouble() * 100.0 <= percentage;

            if (mustReturnValue)
                return value;
            else
                return !value;
        }

        public static DateTime getRandom(this DateTime max, DateTime min, Random random = null)
        {
            return (random ?? currentRandom).nextDate(min, max);
        }

        public static TimeSpan getRandom(this TimeSpan max, Random random = null)
        {
            return (random ?? currentRandom).nextTimeSpan(TimeSpan.Zero, max);
        }

        public static TimeSpan getRandom(this TimeSpan max, TimeSpan min, Random random = null)
        {
            return (random ?? currentRandom).nextTimeSpan(min, max);
        }

        public static double getRandom(this double max, Random random = null)
        {
            return (random ?? currentRandom).nextDouble(0D, max);
        }

        public static double getRandom(this double max, double min, Random random = null)
        {
            return (random ?? currentRandom).nextDouble(min, max);
        }

        public static decimal getRandom(this decimal max, Random random = null)
        {
            return (random ?? currentRandom).nextDecimal(decimal.Zero, max);
        }

        public static decimal getRandom(this decimal max, decimal min, Random random = null)
        {
            return (random ?? currentRandom).nextDecimal(min, max);
        }

        private static int prv_generateSeed()
        {
            long seed = (long)Environment.CurrentManagedThreadId * Environment.TickCount;
            seed = seed % int.MaxValue;
            seed = seed * DateTime.UtcNow.Millisecond;
            seed = seed % int.MaxValue;

            return (int)seed;
        }
    }
}
