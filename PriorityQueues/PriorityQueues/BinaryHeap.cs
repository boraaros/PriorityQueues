using System;
using System.Collections.Generic;

namespace PriorityQueues
{
    public sealed class BinaryHeap<TItem, TPriority> : IPriorityQueue<TItem, TPriority>
    {
        private sealed class BinaryHeapNode : IPriorityQueueEntry<TItem>
        {
            public TItem Item { get; private set; }
            public TPriority Priority { get; internal set; }
            public int Index { get; set; }
            public Guid HeapIdentifier { get; set; }

            public BinaryHeapNode(TItem item, TPriority priority, int index, Guid heapIdentifier)
            {
                Item = item;
                Priority = priority;
                Index = index;
                HeapIdentifier = heapIdentifier;
            }
        }

        private readonly Guid identifier;
        private const int InitialSize = 16;
        private const int Degree = 2;

        private readonly Func<TPriority, TPriority, int> Compare;

        private BinaryHeapNode[] heap;

        public TItem Peek
        {
            get 
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Binary heap does not contain elements");
                }
                return heap[1].Item; 
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
                return heap[1].Priority;
            }
        }

        public int Count { get; private set; }

        public BinaryHeap(PriorityQueueType type, IComparer<TPriority> comparer)
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
            heap = new BinaryHeapNode[InitialSize];
        }

        public BinaryHeap(PriorityQueueType type)
            : this(type, Comparer<TPriority>.Default)
        {
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            for (int i = 1; i <= Count; i++)
            {
                yield return heap[i].Item;
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
            if (Count == heap.Length - 1)
            {
                Array.Resize(ref heap, heap.Length * Degree);
            }
            BinaryHeapNode node = new BinaryHeapNode(item, priority, ++Count, identifier);
            heap[Count] = node;
            HeapifyUp(node);
            return node;
        }

        public TItem Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Binary heap is empty!");
            }
            BinaryHeapNode head = heap[1];
            Remove(heap[1]);
            return head.Item;
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
            BinaryHeapNode node = entry as BinaryHeapNode;
            if (node == null)
            {
                throw new InvalidCastException("Invalid heap entry format!");
            }
            if (node.HeapIdentifier != identifier)
            {
                throw new ArgumentException("Heap does not contain this node!");
            }
            node.Priority = priority;
            HeapifyUp(node);
        }

        public void Remove(IPriorityQueueEntry<TItem> entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            BinaryHeapNode temp = entry as BinaryHeapNode;
            if (temp == null)
            {
                throw new InvalidCastException("Invalid heap entry format!");
            }
            if (temp.HeapIdentifier != identifier)
            {
                throw new ArgumentException("Heap does not contain this node!");
            }
            if (temp.Index == Count)
            {
                temp.HeapIdentifier = Guid.Empty;
                heap[Count--] = null;
                return;
            }
            MoveNode(heap[Count], temp.Index);
            heap[Count--] = null;
            HeapifyUp(heap[HeapifyDown(heap[temp.Index])]);
            temp.HeapIdentifier = Guid.Empty;
        }

        public void Clear()
        {
            for (int i = 1; i <= Count; i++)
            {
                heap[i].HeapIdentifier = Guid.Empty;
            }
            heap = new BinaryHeapNode[InitialSize];
            Count = 0;
        }

        private void HeapifyUp(BinaryHeapNode node)
        {
            BinaryHeapNode parent = Parent(node.Index);
            int to = node.Index;

            while (parent != null && Compare(parent.Priority, node.Priority) > 0)
            {
                int grandParent = parent.Index / Degree;
                int temp = parent.Index;
                MoveNode(parent, to);
                to = temp;
                parent = heap[grandParent];
            }
            MoveNode(node, to);
        }

        private int HeapifyDown(BinaryHeapNode node)
        {
            BinaryHeapNode child = BestChild(node.Index);
            int index = node.Index;

            while (child != null)
            {
                index = child.Index;
                MoveNode(child, child.Index / Degree);
                child = BestChild(index);
            }
            MoveNode(node, index);
            return index;
        }

        private void MoveNode(BinaryHeapNode node, int to)
        {
            heap[to] = node;
            node.Index = to;
        }

        private BinaryHeapNode Parent(int index)
        {
            return index == 1 ? null : heap[index / Degree];
        }

        private BinaryHeapNode BestChild(int index)
        {
            int temp = index * Degree;

            if (Count < temp)
            {
                return null;
            }
            else if (Count == temp)
            {
                return heap[Count];
            }
            else
            {
                return Compare(heap[temp + 1].Priority, heap[temp].Priority) > 0 ?
                heap[temp] : heap[temp + 1];
            }
        }
    }
}
