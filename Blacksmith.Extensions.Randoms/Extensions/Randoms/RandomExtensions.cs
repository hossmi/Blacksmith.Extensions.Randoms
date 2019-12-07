using Blacksmith.Exceptions;
using Blacksmith.Services;
using System;
using System.Collections.Generic;

namespace Blacksmith.Extensions.Randoms
{
    public static class RandomExtensions
    {
        private static Random currentRandom;
        private static IShuffleStrategy currentShuffleStrategy;
        private static int currentShuffleBufferSize;

        static RandomExtensions()
        {
            currentRandom = new Random(prv_generateSeed());
            currentShuffleStrategy = new RandomIterationsShuffleStrategy();
            currentShuffleBufferSize = 1024 * 1024;
        }

        public static Random CurrentRandom
        {
            get => currentRandom;
            set => currentRandom = value ?? throw new ArgumentNullException(nameof(CurrentRandom));
        }

        public static IShuffleStrategy CurrentShuffleStrategy
        {
            get => currentShuffleStrategy;
            set => currentShuffleStrategy = value ?? throw new ArgumentNullException(nameof(CurrentShuffleStrategy));
        }

        public static int CurrentShuffleBufferSize
        {
            get => currentShuffleBufferSize;
            set
            {
                if(value < 2)
                    throw new TooSmallShuffleBufferSize(value);

                currentShuffleBufferSize = value;
            }
        }

        public static DateTime getDateBetween(this Random random, DateTime from, DateTime to)
        {
            DateTime result;
            TimeSpan delta;
            double randomSeconds;

            delta = to - from;

            if (delta.Ticks < 0)
                throw new ArgumentOutOfRangeException();

            randomSeconds = random.NextDouble() * delta.TotalSeconds;
            result = from.AddSeconds(randomSeconds);

            return result;
        }

        public static bool isTrue(this Random random)
        {
            return isTrue(random, 0.5);
        }

        public static bool isTrue(this Random random, double threshold)
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

            range = max - min;
            return min + (decimal)random.NextDouble() * range;
        }

        public static T peekRandom<T>(this IReadOnlyList<T> items, Random random)
        {
            return prv_peekRandom<T>(items, random);
        }

        public static T peekRandom<T>(this IReadOnlyList<T> items)
        {
            return prv_peekRandom<T>(items, currentRandom);
        }

        public static T peekRandom<T>(this IList<T> items, Random random)
        {
            return prv_peekRandom<T>(items, random);
        }

        public static T peekRandom<T>(this IList<T> items)
        {
            return prv_peekRandom<T>(items, currentRandom);
        }

        public static T peekRandom<T>(this T[] items, Random random)
        {
            return prv_peekRandom<T>(items, random);
        }

        public static T peekRandom<T>(this T[] items)
        {
            return prv_peekRandom<T>(items, currentRandom);
        }

        public static T popFromRandom<T>(this IList<T> items, Random random)
        {
            return prv_popFromRandom<T>(items, random);
        }

        public static T popFromRandom<T>(this IList<T> items)
        {
            return prv_popFromRandom<T>(items, currentRandom);
        }

        public static T pushAtRandom<T>(this IList<T> items, T item, Random random)
        {
            return prv_pushAtRandom<T>(items, item, random);
        }

        public static T pushAtRandom<T>(this IList<T> items, T item)
        {
            return prv_pushAtRandom<T>(items, item, currentRandom);
        }

        public static IEnumerable<T> shuffle<T>(this IEnumerable<T> items)
        {
            return prv_shuffle<T>(items, currentRandom, currentShuffleBufferSize);
        }

        public static IEnumerable<T> shuffle<T>(this IEnumerable<T> items, Random random)
        {
            return prv_shuffle<T>(items, random, currentShuffleBufferSize);
        }

        public static IEnumerable<T> shuffle<T>(this IEnumerable<T> items, Random random, int bufferSize)
        {
            return prv_shuffle<T>(items, random, bufferSize);
        }

        private static IEnumerable<T> prv_shuffle<T>(IEnumerable<T> items, Random random, int bufferSize)
        {
            T[] buffer;

            if (bufferSize < 2)
                throw new TooSmallShuffleBufferSize(bufferSize);
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            buffer = new T[bufferSize];
            IEnumerator<T> enumerator = items.GetEnumerator();
            int bufferUpperIndex;

            for (bufferUpperIndex = 0; bufferUpperIndex < bufferSize; bufferUpperIndex++)
            {
                if (enumerator.MoveNext())
                    buffer[bufferUpperIndex] = enumerator.Current;
                else
                    break;
            }

            if (bufferUpperIndex <= 0)
                yield break;

            while (enumerator.MoveNext())
            {
                int nextItem;

                nextItem = random.Next(bufferUpperIndex);
                yield return buffer[nextItem];

                buffer[nextItem] = enumerator.Current;
            }

            for (int i = 0; i < bufferUpperIndex; i++)
                yield return buffer[i];
        }

        private static T prv_peekRandom<T>(IReadOnlyList<T> items, Random random)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            index = prv_getRandomPosition(random, items.Count);

            return items[index];
        }

        private static T prv_peekRandom<T>(IList<T> items, Random random)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            index = prv_getRandomPosition(random, items.Count);

            return items[index];
        }

        private static T prv_peekRandom<T>(T[] items, Random random)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            index = prv_getRandomPosition(random, items.Length);

            return items[index];
        }

        private static T prv_popFromRandom<T>(IList<T> items, Random random)
        {
            int index;
            T item;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            index = prv_getRandomPosition(random, items.Count);
            item = items[index];
            items.RemoveAt(index);

            return item;
        }

        private static T prv_pushAtRandom<T>(IList<T> items, T item, Random random)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            index = prv_getRandomInsertPosition(random, items.Count);
            items.Insert(index, item);

            return item;
        }

        private static int prv_getRandomPosition(Random random, int count)
        {
            if (count <= 0)
                throw new EmptyCollectionException();

            return random.Next(0, count);
        }

        private static int prv_getRandomInsertPosition(Random random, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();

            return random.Next(0, count);
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
