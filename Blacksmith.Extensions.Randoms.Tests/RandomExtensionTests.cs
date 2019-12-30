using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Blacksmith.Extensions.Randoms.Tests
{
    public class RandomExtensionTests
    {
        private const double PRECISION = 1D;
        private readonly Random random;
        private readonly ITestOutputHelper output;

        public RandomExtensionTests(ITestOutputHelper output)
        {
            RandomNumbers.RandomNumberExtensions.CurrentRandom = new Random(0);
            this.random = RandomNumbers.RandomNumberExtensions.CurrentRandom;
            this.output = output;
        }

        [Theory]
        [InlineData(-10.0, 10, 1000)]
        [InlineData(0.0, 0.000001, 1000)]
        public void nextDouble_returns_numbers_in_expected_range(double min, double max, long iterations)
        {
            for (long i = 0; i < iterations; i++)
                Assert.InRange(this.random.nextDouble(min, max), min, max);
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
        public void nextDecimal_returns_numbers_in_expected_range(decimal min, decimal max, long iterations)
        {
            for (long i = 0; i < iterations; i++)
                Assert.InRange(this.random.nextDecimal(min, max), min, max);
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
        public void nextDate_returns_dates_in_expected_range(DateTime min, DateTime max, long iterations)
        {
            for (long i = 0; i < iterations; i++)
                Assert.InRange(this.random.nextDate(min, max), min, max);
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
        [MemberData(nameof(getNextTimespanValidData))]
        public void nextTimeSpan_returns_values_in_expected_range(TimeSpan min, TimeSpan max, long iterations)
        {
            for (long i = 0; i < iterations; i++)
                Assert.InRange(this.random.nextTimeSpan(min, max), min, max);
        }

        public static IEnumerable<object[]> getNextTimespanValidData()
        {
            yield return new object[] { TimeSpan.FromSeconds(34), TimeSpan.FromSeconds(55), 1000 };
            yield return new object[] { TimeSpan.Zero, TimeSpan.FromDays(55), 1000 };
        }

        [Theory]
        [MemberData(nameof(getNextTimespanInvalidData))]
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
