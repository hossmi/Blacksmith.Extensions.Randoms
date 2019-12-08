using Blacksmith.Exceptions;
using Blacksmith.Extensions.Randoms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blacksmith.Algorithms
{
    public class TreeShuffleStrategy : IShuffleStrategy
    {
        public IEnumerable<T> shuffle<T>(IEnumerable<T> items, Random random)
        {
            IEnumerator<T> enumerator;

            if (random == null)
                throw new ArgumentNullException(nameof(random));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            enumerator = items.GetEnumerator();

            if (enumerator.MoveNext())
            {
                PrvTreeNode<T> tree;

                tree = new PrvTreeNode<T>(4, random, enumerator.Current);

                while (enumerator.MoveNext())
                    tree.add(enumerator.Current);

                return tree;
            }
            else
                return Enumerable.Empty<T>();
        }

        private class PrvTreeNode<T> : IEnumerable<T>
        {
            private readonly int childNodeSize;
            private readonly PrvTreeNode<T>[] childs;
            private readonly Random random;
            private readonly T value;

            public PrvTreeNode(int childNodeSize, Random random, T value)
            {
                bool childNodeSizeIsOdd;

                childNodeSizeIsOdd = childNodeSize % 2 != 0;

                if (childNodeSizeIsOdd)
                    throw new ArgumentException($"The {nameof(childNodeSize)} parameter must be odd number greater or equal than 2.", nameof(childNodeSize));

                this.random = random;
                this.value = value;
                this.childNodeSize = childNodeSize;
                this.childs = new PrvTreeNode<T>[this.childNodeSize];
            }

            public void add(T item)
            {
                int selectedChild;
                int[] freePositions;

                freePositions = this.childs
                    .Select((child, index) => child == null ? index : -1)
                    .Where(index => index > -1)
                    .ToArray();

                if(freePositions.Length > 0)
                {
                    selectedChild = freePositions.peekRandom(this.random);
                    this.childs[selectedChild] = new PrvTreeNode<T>(this.childNodeSize, this.random, item);
                }
                else
                {
                    selectedChild = this.random.Next(this.childNodeSize);
                    this.childs[selectedChild].add(item);
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return prv_getEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return prv_getEnumerator();
            }

            private IEnumerator<T> prv_getEnumerator()
            {
                return prv_enumerate().GetEnumerator();
            }

            private IEnumerable<T> prv_enumerate()
            {
                for (int i = 0; i < this.childNodeSize / 2; i++)
                {
                    if (this.childs[i] != null)
                        foreach (T item in this.childs[i])
                            yield return item;
                }

                yield return this.value;

                for (int i = this.childNodeSize / 2; i < this.childNodeSize; i++)
                {
                    if (this.childs[i] != null)
                        foreach (T item in this.childs[i])
                            yield return item;
                }
            }
        }

    }
}