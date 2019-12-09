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

            if (random == null)
                throw new ArgumentNullException(nameof(random));

            delta = to - from;

            if (delta.Ticks <= 0)
                throw new ArgumentOutOfRangeException($"The '{nameof(to)}' parameter must be grater than '{nameof(from)}' parameter.");

            randomSeconds = random.NextDouble() * delta.TotalSeconds;
            result = from.AddSeconds(randomSeconds);

            return result;
        }

        public static TimeSpan nextTimeSpan(this Random random, TimeSpan max)
        {
            return nextTimeSpan(random, TimeSpan.Zero, max);
        }

        public static TimeSpan nextTimeSpan(this Random random, TimeSpan min, TimeSpan max)
        {
            TimeSpan result;
            TimeSpan delta;
            double randomSeconds;

            if (random == null)
                throw new ArgumentNullException(nameof(random));

            delta = max - min;

            if (delta.Ticks <= 0)
                throw new ArgumentOutOfRangeException($"The '{nameof(max)}' parameter must be grater than '{nameof(min)}' parameter.");

            randomSeconds = random.nextDouble(delta.TotalSeconds);
            result = min + TimeSpan.FromSeconds(randomSeconds);

            return result;
        }

        public static double nextDouble(this Random random, double max)
        {
            return nextDouble(random, 0, max);
        }

        public static double nextDouble(this Random random, double min, double max)
        {
            double range;

            if (random == null)
                throw new ArgumentNullException(nameof(random));

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

            if (random == null)
                throw new ArgumentNullException(nameof(random));

            if (max <= min)
                throw new ArgumentOutOfRangeException($"The '{nameof(max)}' parameter must be grater than '{nameof(min)}' parameter.");

            range = max - min;
            return min + (decimal)random.NextDouble() * range;
        }
    }
}
