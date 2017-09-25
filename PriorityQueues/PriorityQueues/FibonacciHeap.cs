using System;
using System.Collections.Generic;

namespace PriorityQueues
{
    public sealed class FibonacciHeap<TItem, TPriority> : IPriorityQueue<TItem, TPriority>
    {
        private sealed class FibonacciNode : IPriorityQueueEntry<TItem>
        {
            public FibonacciNode Parent = null;
            public FibonacciNode Left;
            public FibonacciNode Right;
            public FibonacciNode FirstChild = null;
            public int Degree = 0;
            public bool IsMarked = false;
            public TItem Item { get; internal set; }
            public TPriority Priority { get; internal set; }
            public Guid HeapIdentifier { get; set; }

            public FibonacciNode(TItem item, TPriority priority, Guid heapIdentifier)
            {
                Item = item;
                Priority = priority;
                HeapIdentifier = heapIdentifier;
            }
        }

        private readonly Guid identifier;

        private readonly Func<TPriority, TPriority, int> Compare;

        private FibonacciNode head;

        public TItem Peek
        {
            get 
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Heap contains no elements");
                }
                return head.Item; 
            }
        }

        public TPriority PeekPriority
        {
            get
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Binary heap does not contain elements");
                }
                return head.Priority;
            }
        }

        public int Count { get; private set; }

        public FibonacciHeap(PriorityQueueType type, IComparer<TPriority> comparer = null)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            switch (type)
            {
                case PriorityQueueType.Minimum:
                    Compare = (x, y) => comparer.Compare(x, y);
                    break;
                case PriorityQueueType.Maximum:
                    Compare = (x, y) => comparer.Compare(y, x);
                    break;
                default: throw new ArgumentException(string.Format("Unknown priority queue type: {0}", type));
            }
            identifier = Guid.NewGuid();
        }

        public FibonacciHeap(PriorityQueueType type)
            : this(type, Comparer<TPriority>.Default)
        {
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            foreach (var entry in Enumerate())
            {
                yield return entry.Item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IPriorityQueueEntry<TItem> Enqueue(TItem item, TPriority priority)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (priority == null)
            {
                throw new ArgumentNullException("priority");
            }
            FibonacciNode node = new FibonacciNode(item, priority, identifier);

            if (Count == 0)
            {
                node.Left = node.Right = head = node;
            }
            else
            {
                Paste(head, node);

                if (Compare(head.Priority, priority) > 0)
                {
                    head = node;
                }
            }
            Count++;
            return node;
        }

        public void UpdatePriority(IPriorityQueueEntry<TItem> entry, TPriority priority)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            if (priority == null)
            {
                throw new ArgumentNullException("priority");
            }
            FibonacciNode node = entry as FibonacciNode;
            if (node == null)
            {
                throw new InvalidCastException("Invalid heap entry format!");
            }
            if (node.HeapIdentifier != identifier)
            {
                throw new ArgumentException("Heap does not contain this node!");
            }
            if (node.Parent != null && Compare(node.Parent.Priority, priority) > 0)
            {
                CutNode(node);
            }
            node.Priority = priority;

            if (Compare(head.Priority, priority) > 0)
            {
                head = node;
            }
        }

        public TItem Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Heap is empty!");
            }
            FibonacciNode h = head;

            if (Count == 1)
            {
                head = null;
                Count--;
                h.HeapIdentifier = Guid.Empty;
                return h.Item;
            }
            while (head.Degree > 0)
            {
                CutNode(head.FirstChild);
            }
            Concatenate();
            head.Left.Right = head.Right;
            head.Right.Left = head.Left;
            head = SearchNewMinimum();
            Count--;
            h.HeapIdentifier = Guid.Empty;
            return h.Item;
        }

        public void Remove(IPriorityQueueEntry<TItem> entry)
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

            if (temp.HeapIdentifier != identifier)
            {
                throw new ArgumentException("Heap does not contain this node!");
            }
            CutNode(temp);
            head = temp;
            Dequeue();
        }

        public void Clear()
        {
            foreach (var entry in Enumerate())
            {
                entry.HeapIdentifier = Guid.Empty;
            }
            Count = 0;
            head = null;
        }

        private IEnumerable<FibonacciNode> Enumerate()
        {
            if (head == null)
            {
                yield break;
            }
            var current = head;
            do
            {
                foreach (var node in EnumerateBranch(current))
                {
                    yield return node;
                }
                current = current.Right;
            }
            while (current != head);
        }

        private IEnumerable<FibonacciNode> EnumerateBranch(FibonacciNode root)
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
            yield return root;
        }

        private FibonacciNode SearchNewMinimum()
        {
            FibonacciNode h = head.Right;
            for (FibonacciNode node = head.Right.Right; node != head.Right; node = node.Right)
            {
                if (Compare(h.Priority, node.Priority) > 0)
                {
                    h = node;
                }
            }
            return h;
        }

        private void Concatenate()
        {
            IDictionary<int, FibonacciNode> concat = new Dictionary<int, FibonacciNode>();

            for (FibonacciNode node = head.Right; node != head; )
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
                        if (Compare(n.Priority, node.Priority) > 0)
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
            Paste(head, node);
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
