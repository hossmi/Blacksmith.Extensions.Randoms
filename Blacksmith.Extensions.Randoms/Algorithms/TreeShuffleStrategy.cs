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

                tree = new PrvTreeNode<T>(random, enumerator.Current);

                while (enumerator.MoveNext())
                    tree.add(enumerator.Current);

                return tree;
            }
            else
                return Enumerable.Empty<T>();
        }

        private class PrvTreeNode<T> : IEnumerable<T>
        {
            private readonly Random random;
            private readonly T value;
            private PrvTreeNode<T> leftChild;
            private PrvTreeNode<T> rightChild;

            public PrvTreeNode(Random random, T value)
            {
                this.random = random;
                this.value = value;
                this.leftChild = null;
                this.rightChild = null;
            }

            public void add(T item)
            {
                bool addToLeftChild;

                addToLeftChild = this.random.isTrue();

                if (addToLeftChild)
                {
                    if (this.leftChild == null)
                        this.leftChild = new PrvTreeNode<T>(this.random, item);
                    else
                        this.leftChild.add(item);
                }
                else
                {
                    if (this.rightChild == null)
                        this.rightChild = new PrvTreeNode<T>(this.random, item);
                    else
                        this.rightChild.add(item);
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
                if(this.leftChild != null)
                    foreach (T item in this.leftChild)
                        yield return item;

                yield return this.value;

                if (this.rightChild != null)
                    foreach (T item in this.rightChild)
                        yield return item;
            }
        }

    }
}