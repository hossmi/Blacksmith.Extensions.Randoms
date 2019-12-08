using Blacksmith.Exceptions;
using Blacksmith.Extensions.Randoms;
using Blacksmith.Extensions.ShuffledCollections;
using System;
using System.Collections.Generic;

namespace Blacksmith.Algorithms
{
    public class ListShuffleStrategy : IShuffleStrategy
    {
        private int bufferSize;

        public ListShuffleStrategy(int bufferSize)
        {
            if (bufferSize < 2)
                throw new TooSmallShuffleBufferSize(bufferSize);

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
                    buffer.pushAtRandomPosition(item);
                }
                else
                    break;
            }

            while(enumerator.MoveNext())
            {
                T item;
                int randomIndex;

                item = enumerator.Current;
                randomIndex = random.Next(0, this.bufferSize);
                yield return buffer[randomIndex];

                buffer[randomIndex] = item;
            }

            while (buffer.Count > 0)
                yield return buffer.popFromRandomPosition<T>(random);
        }
    }
}