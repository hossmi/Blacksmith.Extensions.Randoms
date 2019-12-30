using Blacksmith.Algorithms;
using System;
using System.Collections.Generic;

namespace Blacksmith.Extensions.ShuffledEnumerableCollections
{
    public static class ShuffleEnumerableExtensions
    {
        private static IShuffleStrategy currentShuffleStrategy;

        static ShuffleEnumerableExtensions()
        {
            currentShuffleStrategy = new ListShuffleStrategy(1024);
        }

        public static IShuffleStrategy CurrentShuffleStrategy
        {
            get => currentShuffleStrategy;
            set => currentShuffleStrategy = value ?? throw new ArgumentNullException(nameof(CurrentShuffleStrategy));
        }

        public static IEnumerable<T> shuffle<T>(this IEnumerable<T> items, Random random = null, IShuffleStrategy shuffleStrategy = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            random = random ?? RandomNumbers.RandomNumberExtensions.CurrentRandom;
            shuffleStrategy = shuffleStrategy ?? CurrentShuffleStrategy;

            return shuffleStrategy.shuffle<T>(items, random);
        }
    }
}
