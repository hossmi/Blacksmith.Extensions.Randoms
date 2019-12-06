using System;

namespace Blacksmith.Services
{
    public interface IShuffleStrategy
    {
        void shuffle<T>(T[] items, Random random);
    }
}