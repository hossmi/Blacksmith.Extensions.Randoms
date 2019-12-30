using AutoFixture.Xunit2;
using Xunit;
using Blacksmith.Extensions.RandomNumbers;
using FluentAssertions;
using System;

namespace Blacksmith.Extensions.Randoms.Tests
{
    public class AutoTests
    {
        [Theory]
        [AutoData]
        public void boolean_at_50_returns_same_amount_trues_and_falses(int iterations)
        {
            int trues, falses;

            trues = 0;
            falses = 0;

            for (int i = 0; i < iterations; i++)
            {
                if (true.at(50))
                    trues++;
                else
                    falses++;
            }

            trues
                .Should()
                .BeGreaterThan(iterations * 1 / 3)
                .And
                .BeLessThan(iterations * 2 / 3);

            falses
                .Should()
                .BeGreaterThan(iterations * 1 / 3)
                .And
                .BeLessThan(iterations * 2 / 3);
        }

        [Theory]
        [AutoData]
        public void nextDate_returns_value_in_expected_range(DateTime max, TimeSpan delta)
        {
            DateTime min;

            min = max - delta;

            max.getRandom(min)
                .Should()
                .BeOnOrAfter(min)
                .And
                .BeOnOrBefore(max);
        }

        [Theory]
        [AutoData]
        public void nextTimespan_returns_value_in_expected_range(TimeSpan max)
        {
            TimeSpan min;

            min = max.getRandom();

            min.Should()
                .BeGreaterOrEqualTo(TimeSpan.Zero)
                .And
                .BeLessOrEqualTo(max);

            max.getRandom(min)
                .Should()
                .BeGreaterOrEqualTo(min)
                .And
                .BeLessOrEqualTo(max);
        }

        [Theory]
        [AutoData]
        public void nextDecimal_returns_value_in_expected_range(decimal max)
        {
            decimal min;

            min = max.getRandom();

            min.Should()
                .BeGreaterOrEqualTo(0m)
                .And
                .BeLessOrEqualTo(max);

            max.getRandom(min)
                .Should()
                .BeGreaterOrEqualTo(min)
                .And
                .BeLessOrEqualTo(max);
        }

        [Theory]
        [AutoData]
        public void nextDouble_returns_value_in_expected_range(double max)
        {
            double min;

            min = max.getRandom();

            min.Should()
                .BeGreaterOrEqualTo(0d)
                .And
                .BeLessOrEqualTo(max);

            max.getRandom(min)
                .Should()
                .BeGreaterOrEqualTo(min)
                .And
                .BeLessOrEqualTo(max);
        }
    }
}
