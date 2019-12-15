using AutoFixture.Xunit2;
using Xunit;
using Blacksmith.Extensions.Randoms;
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
        public void nextDate_returns_value_in_expected_range(DateTime from, TimeSpan to)
        {
            Random r;
            DateTime randomValue;

            r = new Random();

            randomValue = r.nextDate(from, from + to);

            randomValue.Should()
                .BeAfter(from - new TimeSpan(1))
                .And
                .BeBefore(from + to);
        }

        [Theory]
        [AutoData]
        public void nextTimespan_returns_value_in_expected_range(TimeSpan from, TimeSpan to)
        {
            Random r;
            TimeSpan randomValue;

            r = new Random();

            randomValue = r.nextTimeSpan(from, from + to);

            randomValue.Should()
                .BeGreaterOrEqualTo(from)
                .And
                .BeLessOrEqualTo(from + to);
        }

        [Theory]
        [AutoData]
        public void nextDecimal_returns_value_in_expected_range(decimal from, decimal to)
        {
            Random r;
            decimal randomValue;

            r = new Random();

            randomValue = r.nextDecimal(from, from + to);

            randomValue.Should()
                .BeGreaterOrEqualTo(from)
                .And
                .BeLessOrEqualTo(from + to);
        }

        [Theory]
        [AutoData]
        public void nextDouble_returns_value_in_expected_range(double from, double to)
        {
            Random r;
            double randomValue;

            r = new Random();

            randomValue = r.nextDouble(from, from + to);

            randomValue.Should()
                .BeGreaterOrEqualTo(from)
                .And
                .BeLessOrEqualTo(from + to);
        }
    }
}
