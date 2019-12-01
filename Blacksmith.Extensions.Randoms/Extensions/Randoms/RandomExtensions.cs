using Blacksmith.Exceptions;
using System;
using System.Collections.Generic;

namespace Blacksmith.Extensions.Randoms
{
    public static class RandomExtensions
    {
        private static Random random;

        static RandomExtensions()
        {
            int seed;

            seed = (int)(((long)Environment.CurrentManagedThreadId * (long)Environment.TickCount) % (long)int.MaxValue);

            random = new Random(seed);
        }

        public static Random Instance
        {
            get
            {
                return random;
            }
            set
            {
                random = value ?? throw new ArgumentNullException(nameof(Instance));
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
            return random.NextDouble() >= 0.5;
        }

        public static bool isTrue(this Random random, double threshold)
        {
            return random.NextDouble() >= (1.0 - threshold);
        }

        public static double nextDouble(this Random random, double max)
        {
            return random.NextDouble() * max;
        }

        public static decimal nextDecimal(this Random random, decimal max)
        {
            return (decimal)random.NextDouble() * max;
        }

        public static T peekRandom<T>(this IReadOnlyList<T> items, Random random)
        {
            return prv_peekRandom<T>(items, random);
        }

        public static T peekRandom<T>(this IReadOnlyList<T> items)
        {
            return prv_peekRandom<T>(items, Instance);
        }

        public static T peekRandom<T>(this IList<T> items, Random random)
        {
            return prv_peekRandom<T>(items, random);
        }

        public static T peekRandom<T>(this IList<T> items)
        {
            return prv_peekRandom<T>(items, Instance);
        }

        public static T peekRandom<T>(this T[] items, Random random)
        {
            return prv_peekRandom<T>(items, random);
        }

        public static T peekRandom<T>(this T[] items)
        {
            return prv_peekRandom<T>(items, Instance);
        }


        public static T popRandom<T>(this IList<T> items, Random random)
        {
            return prv_popRandom<T>(items, random);
        }

        public static T popRandom<T>(this IList<T> items)
        {
            return prv_popRandom<T>(items, Instance);
        }



        public static void shuffle<T>(this T[] items, Random random)
        {
            int n = random.Next(0, items.Length);

            for (int i = 0; i < n; i++)
            {
                int x = random.Next(0, items.Length);
                int y = random.Next(0, items.Length);

                T item;

                item = items[x];
                items[x] = items[y];
                items[y] = item;
            }
        }

        public static IEnumerable<T> getShuffled<T>(this IEnumerable<T> items, Random random, int bufferSize)
        {
            T[] buffer = new T[bufferSize];
            IEnumerator<T> enumerator = items.GetEnumerator();
            int itemCount;

            for (itemCount = 0; itemCount < bufferSize; itemCount++)
            {
                if (enumerator.MoveNext())
                    buffer[itemCount] = enumerator.Current;
                else
                    break;
            }

            if (itemCount <= 0)
                yield break;

            while (enumerator.MoveNext())
            {
                int nextItem = random.Next(itemCount);
                yield return buffer[nextItem];
                buffer[nextItem] = enumerator.Current;
            }

            for (int i = 0; i < itemCount; i++)
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

        private static T prv_popRandom<T>(IList<T> items, Random random)
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

        private static int prv_getRandomPosition(Random random, int count)
        {
            if (count <= 0)
                throw new EmptyCollectionException();

            return random.Next(0, count);
        }
    }
}
