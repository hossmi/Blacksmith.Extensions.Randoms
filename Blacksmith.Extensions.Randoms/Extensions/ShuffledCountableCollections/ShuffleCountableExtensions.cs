using Blacksmith.Algorithms;
using Blacksmith.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blacksmith.Extensions.ShuffledCountableCollections
{
    public static class ShuffleCountableExtensions
    {
        public static T peekRandom<T>(this IReadOnlyList<T> items, Random random)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? RandomNumbers.RandomNumberExtensions.CurrentRandom;
            index = prv_getRandomPosition(random, items.Count);

            return items[index];
        }

        public static T peekRandom<T>(this IList<T> items, Random random)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? RandomNumbers.RandomNumberExtensions.CurrentRandom;
            index = prv_getRandomPosition(random, items.Count);

            return items[index];
        }

        public static T peekRandom<T>(this T[] items, Random random = null)
        {
            int index;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? RandomNumbers.RandomNumberExtensions.CurrentRandom;
            index = prv_getRandomPosition(random, items.Length);

            return items[index];
        }

        public static T popFromRandomPosition<T>(this IList<T> items, Random random = null)
        {
            int index;
            T item;

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? RandomNumbers.RandomNumberExtensions.CurrentRandom;

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

            random = random ?? RandomNumbers.RandomNumberExtensions.CurrentRandom;

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
    }
}
