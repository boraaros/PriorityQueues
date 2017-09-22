using System;
using System.Collections.Generic;

namespace PriorityQueues
{
    public sealed class FibonacciHeap<TItem, TPriority> : IPriorityQueue<TItem, TPriority>
    {
        private sealed class FibonacciNode : IHeapEntry<TItem, TPriority>
        {
            public FibonacciNode Parent = null;
            public FibonacciNode Left;
            public FibonacciNode Right;
            public FibonacciNode FirstChild = null;
            public int Degree = 0;
            public bool IsMarked = false;
            public TItem Item { get; internal set; }
            public TPriority Priority { get; internal set; }

            public FibonacciNode(TItem item, TPriority priority)
            {
                Item = item;
                Priority = priority;
            }
        }

        private IComparer<TPriority> comparer;

        private FibonacciNode minimum;

        public IHeapEntry<TItem, TPriority> Minimum
        {
            get 
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Heap contains no elements");
                }
                return (IHeapEntry<TItem, TPriority>)minimum; 
            }
        }

        public int Count { get; private set; }

        public FibonacciHeap(IComparer<TPriority> comparer = null)
        {
            if (comparer != null)
            {
                this.comparer = comparer;
            }
            else
            {
                this.comparer = Comparer<TPriority>.Default;
            }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            if (minimum == null)
            {
                yield break;
            }
            var current = minimum;
            do
            {
                foreach (var node in EnumerateBranch(current))
                {
                    yield return node;
                }
                current = current.Right;
            }
            while (current != minimum);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IHeapEntry<TItem, TPriority> Insert(TItem item, TPriority priority)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (priority == null)
            {
                throw new ArgumentNullException("priority");
            }
            FibonacciNode node = new FibonacciNode(item, priority);

            if (Count == 0)
            {
                node.Left = node.Right = minimum = node;
            }
            else
            {
                Paste(minimum, node);

                if (comparer.Compare(minimum.Priority, priority) > 0)
                {
                    minimum = node;
                }
            }
            Count++;
            return node;
        }

        public void Increase(IHeapEntry<TItem, TPriority> entry, TPriority priority)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            if (priority == null)
            {
                throw new ArgumentNullException("priority");
            }
            if (comparer.Compare(priority, entry.Priority) > 0)
            {
                throw new ArgumentException("Invalid new priority!");
            }

            FibonacciNode node = entry as FibonacciNode;
            if (node == null)
            {
                throw new InvalidCastException("Invalid heap entry format!");
            }

            if (node.Parent != null && comparer.Compare(node.Parent.Priority, priority) > 0)
            {
                CutNode(node);
            }
            node.Priority = priority;

            if (comparer.Compare(minimum.Priority, priority) > 0)
            {
                minimum = node;
            }
        }

        public IHeapEntry<TItem, TPriority> RemoveMinimum()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Heap is empty!");
            }
            FibonacciNode min = minimum;

            if (Count == 1)
            {
                minimum = null;
                Count--;
                min.Left = min.Right = null;
                return min;
            }
            while (minimum.Degree > 0)
            {
                CutNode(minimum.FirstChild);
            }
            Concatenate();
            minimum.Left.Right = minimum.Right;
            minimum.Right.Left = minimum.Left;
            minimum = SearchNewMinimum();
            Count--;
            min.Left = min.Right = null;
            return min;
        }

        public void Remove(IHeapEntry<TItem, TPriority> entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            FibonacciNode temp = entry as FibonacciNode;
            if (temp == null)
            {
                throw new InvalidCastException("Invalid heap entry format!");
            }

            if (temp.Left == null)
            {
                throw new ArgumentException("Heap does not contain this node!");
            }
            CutNode(temp);
            minimum = temp;
            RemoveMinimum();
        }

        private IEnumerable<TItem> EnumerateBranch(FibonacciNode root)
        {
            if (root.FirstChild != null)
            {
                var current = root.FirstChild;
                do
                {
                    foreach (var node in EnumerateBranch(current))
                    {
                        yield return node;
                    }
                    current = current.Right;
                }
                while (current != root.FirstChild);
            }
            yield return root.Item;
        }

        private FibonacciNode SearchNewMinimum()
        {
            FibonacciNode min = minimum.Right;
            for (FibonacciNode node = minimum.Right.Right; node != minimum.Right; node = node.Right)
            {
                if (comparer.Compare(min.Priority, node.Priority) > 0)
                {
                    min = node;
                }
            }
            return min;
        }

        private void Concatenate()
        {
            IDictionary<int, FibonacciNode> concat = new Dictionary<int, FibonacciNode>();

            for (FibonacciNode node = minimum.Right; node != minimum; )
            {
                FibonacciNode next = node.Right;
                bool cont = true;
                do
                {
                    cont = true;
                    if (!concat.ContainsKey(node.Degree))
                    {
                        concat.Add(node.Degree, node);
                        cont = false;
                    }
                    else
                    {
                        FibonacciNode n = concat[node.Degree];
                        concat.Remove(node.Degree);
                        if (comparer.Compare(n.Priority, node.Priority) > 0)
                        {
                            Merge(node, n);
                        }
                        else
                        {
                            Merge(n, node);
                            node = n;
                        }
                    }
                }
                while (cont);
                node = next;
            }
        }

        private void Merge(FibonacciNode root, FibonacciNode child)
        {
            child.Parent = root;
            child.IsMarked = false;
            child.Left.Right = child.Right;
            child.Right.Left = child.Left;
            if (root.Degree == 0)
            {
                root.FirstChild = child;
                child.Left = child.Right = child;
            }
            else
            {
                Paste(root.FirstChild, child);
            }
            root.Degree++;
        }

        private void Paste(FibonacciNode prev, FibonacciNode next)
        {
            next.Left = prev;
            next.Right = prev.Right;
            prev.Right.Left = next;
            prev.Right = next;
        }

        private void CutNode(FibonacciNode node)
        {
            if (node.Parent == null)
            {
                return;
            }
            else if (node.Parent.Degree == 1)
            {
                node.Parent.FirstChild = null;
            }
            else if (node.Parent.FirstChild == node)
            {
                node.Parent.FirstChild = node.Right;
                node.Right.Left = node.Left;
                node.Left.Right = node.Right;
            }
            else
            {
                node.Right.Left = node.Left;
                node.Left.Right = node.Right;
            }
            Paste(minimum, node);
            node.Parent.Degree--;
            if (node.Parent.IsMarked)
            {
                CutNode(node.Parent);
            }
            else
            {
                node.Parent.IsMarked = true;
            }
            node.Parent = null;
        }
    }
}
