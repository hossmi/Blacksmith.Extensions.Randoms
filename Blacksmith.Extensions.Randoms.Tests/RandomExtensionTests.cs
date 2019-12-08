using Blacksmith.Extensions.ShuffledCollections;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Blacksmith.Extensions.Randoms.Tests
{
    public class RandomExtensionTests
    {
        private static readonly double PRECISION;
        private readonly Random random;
        private readonly ITestOutputHelper output;

        static RandomExtensionTests()
        {
#if DEBUG
            PRECISION = 0.001;
#else
            PRECISION = 0.0001;
#endif
        }

        public RandomExtensionTests(ITestOutputHelper output)
        {
            this.random = ShuffleExtensions.CurrentRandom;
            this.output = output;
        }

        [Theory]
        [InlineData(0.999, 1000)]
        [InlineData(0.999, 10 * 1000L)]
        [InlineData(0.999, 100 * 1000L)]
        [InlineData(0.999, 1000 * 1000L)]
        [InlineData(0.999, 10 * 1000L * 1000L)]
        [InlineData(0.999, 100 * 1000L * 1000L)]
        [InlineData(0.999, 1000 * 1000L * 1000L)]
        public void isTrue_returns_same_amount_of_trues_and_falses(double truePercentage, long iterations)
        {
            bool proportionIsCloseToTruePercentage;
            double proportion;
            Random r;
            long trues, falses;

            r = ShuffleExtensions.CurrentRandom;
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
        [InlineData(-10.0, 10, 1000)]
        [InlineData(0.0, 0.000001, 1000)]
        public void nextDouble_returns_numbers_in_expected_range(double min, double max, int iterations)
        {
            for (int i = 0; i < iterations; i++)
                Assert.InRange<double>(this.random.nextDouble(min, max), min, max);
        }

        [Theory]
        [InlineData(10.0, -10.0)]
        [InlineData(0.0, 0.0)]
        public void nextDouble_throws_exception_on_invalid_range(double min, double max)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => this.random.nextDouble(min, max));
        }

        [Theory]
        [InlineData(-10.0, 10, 1000)]
        [InlineData(0.0, 0.000001, 1000)]
        public void nextDecimal_returns_numbers_in_expected_range(decimal min, decimal max, int iterations)
        {
            for (int i = 0; i < iterations; i++)
                Assert.InRange<decimal>(this.random.nextDecimal(min, max), min, max);
        }

        [Theory]
        [InlineData(10.0, -10.0)]
        [InlineData(0.0, 0.0)]
        public void nextDecimal_throws_exception_on_invalid_range(decimal min, decimal max)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => this.random.nextDecimal(min, max));
        }

        [Theory]
        [MemberData(nameof(getNextDateValidData))]
        public void nextDate_returns_dates_in_expected_range(DateTime min, DateTime max, int iterations)
        {
            for (int i = 0; i < iterations; i++)
                Assert.InRange<DateTime>(this.random.nextDate(min, max), min, max);
        }

        public static IEnumerable<object[]> getNextDateValidData()
        {
            yield return new object[] { DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(10), 1000 };
            yield return new object[] { new DateTime(2001, 1, 1), new DateTime(2010, 12, 31), 1000 };
        }

        [Theory]
        [MemberData(nameof(getNextDateInvalidData))]
        public void nextDate_throws_exception_on_invalid_range(DateTime min, DateTime max)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => this.random.nextDate(min, max));
        }

        public static IEnumerable<object[]> getNextDateInvalidData()
        {
            yield return new object[] { DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(-10) };
            yield return new object[] { new DateTime(2001, 1, 1), new DateTime(2001, 1, 1) };
        }

        [Theory]
        [MemberData(nameof(getNextDateValidData))]
        public void nextTimeSpan_returns_values_in_expected_range(TimeSpan min, TimeSpan max, int iterations)
        {
            for (int i = 0; i < iterations; i++)
                Assert.InRange<TimeSpan>(this.random.nextTimeSpan(min, max), min, max);
        }

        public static IEnumerable<object[]> getNextTimespanValidData()
        {
            yield return new object[] { TimeSpan.FromSeconds(34), TimeSpan.FromSeconds(55), 1000 };
            yield return new object[] { TimeSpan.Zero, TimeSpan.FromDays(55), 1000 };
        }

        [Theory]
        [MemberData(nameof(getNextDateInvalidData))]
        public void nextTimeSpan_throws_exception_on_invalid_range(TimeSpan min, TimeSpan max)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => this.random.nextTimeSpan(min, max));
        }

        public static IEnumerable<object[]> getNextTimespanInvalidData()
        {
            yield return new object[] { TimeSpan.FromSeconds(55), TimeSpan.FromSeconds(34) };
            yield return new object[] { TimeSpan.Zero, TimeSpan.Zero };
            yield return new object[] { TimeSpan.FromDays(1), TimeSpan.FromDays(1) };
        }
    }
}
