using System;

namespace Blacksmith.Extensions.Randoms
{
    public static class RandomExtensions
    {
        public static DateTime nextDate(this Random random, DateTime from, DateTime to)
        {
            DateTime result;
            TimeSpan delta;
            double randomSeconds;

            delta = to - from;

            if (delta.Ticks < 0)
                throw new ArgumentOutOfRangeException($"The '{nameof(to)}' parameter must be grater than '{nameof(from)}' parameter.");

            randomSeconds = random.NextDouble() * delta.TotalSeconds;
            result = from.AddSeconds(randomSeconds);

            return result;
        }

        public static bool isTrue(this Random random, double threshold = 0.5d)
        {
            return random.NextDouble() >= (1.0 - threshold);
        }

        public static double nextDouble(this Random random, double max)
        {
            return nextDouble(random, 0, max);
        }

        public static double nextDouble(this Random random, double min, double max)
        {
            double range;

            if (max <= min)
                throw new ArgumentOutOfRangeException($"The '{nameof(max)}' parameter must be grater than '{nameof(min)}' parameter.");

            range = max - min;
            return min + random.NextDouble() * range;
        }

        public static decimal nextDecimal(this Random random, decimal max)
        {
            return nextDecimal(random, decimal.Zero, max);
        }

        public static decimal nextDecimal(this Random random, decimal min, decimal max)
        {
            decimal range;

            if (max <= min)
                throw new ArgumentOutOfRangeException($"The '{nameof(max)}' parameter must be grater than '{nameof(min)}' parameter.");

            range = max - min;
            return min + (decimal)random.NextDouble() * range;
        }
    }
}
