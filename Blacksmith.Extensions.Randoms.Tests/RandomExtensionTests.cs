using Blacksmith.Extensions.ShuffledCollections;
using System;
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
    }
}
