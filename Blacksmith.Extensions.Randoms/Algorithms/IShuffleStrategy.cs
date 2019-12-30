using System;
using System.Collections.Generic;

namespace Blacksmith.Algorithms
{
    public interface IShuffleStrategy
    {
        IEnumerable<T> shuffle<T>(IEnumerable<T> items, Random random);
    }
}