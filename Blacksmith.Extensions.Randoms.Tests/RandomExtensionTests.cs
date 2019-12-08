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
            this.output = output;
        }

        [Theory]
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
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(10.0, -10.0)]
        [InlineData(0.0, 0.0)]
        public void nextDouble_throws_exception_on_invalid_range(double min, double max)
        {
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(-10.0, 10, 1000)]
        [InlineData(0.0, 0.000001, 1000)]
        public void nextDecimal_returns_numbers_in_expected_range(decimal min, decimal max, int iterations)
        {
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(10.0, -10.0)]
        [InlineData(0.0, 0.0)]
        public void nextDecimal_throws_exception_on_invalid_range(double min, double max)
        {
            throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(getNextDateValidData))]
        public void nextDate_returns_dates_in_expected_range(DateTime min, DateTime max, int iterations)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<object[]> getNextDateValidData()
        {
            throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(getNextDateInvalidData))]
        public void nextDate_throws_exception_on_invalid_range(DateTime min, DateTime max)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<object[]> getNextDateInvalidData()
        {
            throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(getNextDateValidData))]
        public void nextTimeSpan_returns_values_in_expected_range(TimeSpan min, TimeSpan max, int iterations)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<object[]> getNextTimespanValidData()
        {
            throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(getNextDateInvalidData))]
        public void nextTimeSpan_throws_exception_on_invalid_range(TimeSpan min, TimeSpan max)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<object[]> getNextTimespanInvalidData()
        {
            throw new NotImplementedException();
        }
    }
}
