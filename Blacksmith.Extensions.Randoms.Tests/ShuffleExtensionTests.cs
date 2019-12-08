using Blacksmith.Extensions.ShuffledCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Blacksmith.Extensions.Randoms.Tests
{
    public class ShuffleExtensionTests
    {
        private static readonly double PRECISION;
        private readonly ITestOutputHelper output;

        static ShuffleExtensionTests()
        {
#if DEBUG
            PRECISION = 0.001;
#else
            PRECISION = 0.0001;
#endif
        }

        public ShuffleExtensionTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(1000L)]
        [InlineData(10 * 1000L)]
        [InlineData(100 * 1000L)]
        [InlineData(1000 * 1000L)]
        [InlineData(10 * 1000L * 1000L)]
        [InlineData(100 * 1000L * 1000L)]
        [InlineData(1000 * 1000L * 1000L)]
        public void peekRandom_returns_each_element_in_same_proportion(long iterations)
        {
            bool allDeviationsAreCloseToZero;
            string[] names;
            IDictionary<string, long> choosenNames;
            double average;

            names = new string[]
            {
                "pepe",
                "tronco",
                "Rosa",
                "Irene",
                "Lucas",
                "Narciso",
            };

            choosenNames = new Dictionary<string, long>();

            foreach (string name in names)
                choosenNames.Add(name, 0);

            for (int i = 0; i < iterations; ++i)
            {
                string name;

                name = names.peekRandom();
                choosenNames[name] = choosenNames[name] + 1;
            }

            average = (double)iterations / (double)names.Length;

            allDeviationsAreCloseToZero = choosenNames
                .Values
                .Select(v => (double)v)
                .Select(v => (v - average) / average)
                .Select(Math.Abs)
                .All(deviation =>
                {
                    this.output.WriteLine($"Deviation {deviation,-18}, Precission {PRECISION}");
                    return deviation < PRECISION;
                });

            Assert.True(allDeviationsAreCloseToZero);
        }

        [Theory]
        [MemberData(nameof(getShuffleTestData))]
        public void shuffle_return_completely_unordered_collections(int arraySize, double unorderedPercentage, int bufferSize)
        {
            int[] array;
            int unordered;
            double unorderedProportion;
            bool arrayIsMessyEnough;

            ShuffleExtensions.CurrentShuffleStrategy = new Algorithms.ListShuffleStrategy(bufferSize);

            array = Enumerable
                .Range(0, arraySize)
                .shuffle()
                .ToArray();

            unordered = 0;

            for (int i = 0; i < arraySize; ++i)
                if (array[i] != i)
                    unordered++;

            unorderedProportion = (double)unordered / (double)arraySize;
            arrayIsMessyEnough = unorderedProportion >= unorderedPercentage;
            Assert.Equal(arraySize, array.Length);
            Assert.True(arrayIsMessyEnough);
        }

        public static IEnumerable<object[]> getShuffleTestData()
        {
            yield return new object[] { 100000000, 0.99D, 64 };
            yield return new object[] { 100000000, 0.99D, 128 };
            yield return new object[] { 100000000, 0.99D, 256 };
            yield return new object[] { 100000000, 0.99D, 512 };
            yield return new object[] { 100000000, 0.99D, 1024 };
            yield return new object[] { 100000000, 0.99D, 4 * 1024 };
            yield return new object[] { 100000000, 0.99D, 16 * 1024 };
            yield return new object[] { 100000000, 0.99D, 32 * 1024 };
            yield return new object[] { 100000000, 0.99D, 64 * 1024 };
        }
    }
}
