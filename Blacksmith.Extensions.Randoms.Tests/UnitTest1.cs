using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Blacksmith.Extensions.Randoms.Tests
{
    public class UnitTest1
    {
        private static readonly double PRECISION;

        private readonly ITestOutputHelper output;

        static UnitTest1()
        {
#if DEBUG
            PRECISION = 0.001;
#else
            PRECISION = 0.0001;
#endif
        }

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(0.0, 1000000000L)]
        [InlineData(0.236, 1000000000L)]
        [InlineData(0.382, 1000000000L)]
        [InlineData(0.5, 1000000000L)]
        [InlineData(0.618, 1000000000L)]
        [InlineData(0.762, 1000000000L)]
        [InlineData(1, 1000000000L)]
        public void isTrue_returns_same_amount_of_trues_and_falses(double truePercentage, long iterations)
        {
            bool proportionIsCloseToTruePercentage;
            double proportion;
            Random r;
            long trues, falses;

            r = RandomExtensions.CurrentRandom;
            trues = 0;
            falses = 0;

            for (long i = 0; i < iterations; ++i)
            {
                if (r.isTrue(truePercentage))
                    ++trues;
                else
                    ++falses;
            }

            proportion = (double)trues / ((double)falses + (double)trues);
            this.output.WriteLine($"Proportion {proportion,-20}, Expected {truePercentage}");

            proportion = proportion - truePercentage;
            proportion = Math.Abs(proportion);
            proportionIsCloseToTruePercentage = proportion <= PRECISION;

            Assert.True(proportionIsCloseToTruePercentage);
        }

        [Theory]
        [InlineData(1000000000L)]
        public void next_for_randoms_returns_each_element_equally_after_1_minute_of_execution(long iterations)
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
        [InlineData(1000000, 0.5D)]
        [InlineData(1000000, 0.75D)]
        [InlineData(1000000, 0.85D)]
        [InlineData(1000000, 0.95D)]
        [InlineData(1000000, 1.0D)]
        [InlineData(10000000, 0.95D)]
        public void an_array_must_be_completely_unordered_after_shuffle_method_call(int arraySize, double unorderedPercentage)
        {
            int[] array;
            int unordered;
            double unorderedProportion;
            bool arrayIsMessyEnough;

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
    }
}
