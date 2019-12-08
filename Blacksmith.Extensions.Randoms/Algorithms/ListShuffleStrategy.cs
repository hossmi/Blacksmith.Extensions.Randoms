using Blacksmith.Exceptions;
using Blacksmith.Extensions.Randoms;
using System;
using System.Collections.Generic;

namespace Blacksmith.Algorithms
{
    public class ListShuffleStrategy : IShuffleStrategy
    {
        private readonly Random random;
        private int bufferSize;

        public ListShuffleStrategy(Random random, int bufferSize)
        {
            if (bufferSize < 2)
                throw new TooSmallShuffleBufferSize(bufferSize);
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            this.random = random;
            this.bufferSize = bufferSize;
        }

        public IEnumerable<T> shuffle<T>(IEnumerable<T> items, Random random)
        {
            IEnumerator<T> enumerator;
            IList<T> buffer;

            if (random == null)
                throw new ArgumentNullException(nameof(random));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            enumerator = items.GetEnumerator();
            buffer = new List<T>(this.bufferSize);

            for (int i = 0; i < this.bufferSize; i++)
            {
                if (enumerator.MoveNext())
                {
                    T item;

                    item = enumerator.Current;
                    buffer.pushAtRandom(item);
                }
                else
                    break;
            }

            while(enumerator.MoveNext())
            {
                T item;
                int randomIndex;

                item = enumerator.Current;
                randomIndex = this.random.Next(0, this.bufferSize);
                yield return buffer[randomIndex];

                buffer[randomIndex] = item;
            }

            while (buffer.Count > 0)
                yield return buffer.popFromRandom<T>(this.random);
        }
    }
}