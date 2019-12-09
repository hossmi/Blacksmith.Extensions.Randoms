using Blacksmith.Algorithms;
using Blacksmith.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blacksmith.Extensions.ShuffledCollections
{
    public static class ShuffleExtensions
    {
        private static Random currentRandom;
        private static IShuffleStrategy currentShuffleStrategy;

        static ShuffleExtensions()
        {
            currentRandom = new Random(prv_generateSeed());
            currentShuffleStrategy = new ListShuffleStrategy(1024);
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

        public static bool almostAt(this bool value, double percentage = 50.0, Random random = null)
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

        public static T peekRandom<T>(this IReadOnlyList<T> items, Random random)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? currentRandom;
            index = prv_getRandomPosition(random, items.Count);

            return items[index];
        }

        public static T peekRandom<T>(this IList<T> items, Random random)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? currentRandom;
            index = prv_getRandomPosition(random, items.Count);

            return items[index];
        }

        public static T peekRandom<T>(this T[] items, Random random = null)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? currentRandom;
            index = prv_getRandomPosition(random, items.Length);

            return items[index];
        }

        public static T popFromRandomPosition<T>(this IList<T> items, Random random = null)
        {
            int index;
            T item;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? currentRandom;

            index = prv_getRandomPosition(random, items.Count);
            item = items[index];
            items.RemoveAt(index);

            return item;
        }

        public static T pushAtRandomPosition<T>(this IList<T> items, T item, Random random = null)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? currentRandom;

            index = prv_getRandomInsertPosition(random, items.Count);
            items.Insert(index, item);

            return item;
        }

        public static IEnumerable<T> shuffle<T>(this IEnumerable<T> items, Random random = null, IShuffleStrategy shuffleStrategy = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? currentRandom;
            shuffleStrategy = shuffleStrategy ?? CurrentShuffleStrategy;

            return shuffleStrategy.shuffle<T>(items, random);
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
