using System;

namespace Blacksmith.Services
{
    public interface IShuffleStrategy
    {
        void shuffle<T>(T[] items, Random random);
    }

    public class RandomIterationsShuffleStrategy : IShuffleStrategy
    {
        public void shuffle<T>(T[] items, Random random)
        {
            int iterations; 
            
            iterations = random.Next(0, items.Length);

            for (int i = 0; i < iterations; ++i)
            {
                int x, y;

                x = random.Next(0, items.Length);
                y = random.Next(0, items.Length);

                T item;

                item = items[x];
                items[x] = items[y];
                items[y] = item;
            }
        }
    }
}