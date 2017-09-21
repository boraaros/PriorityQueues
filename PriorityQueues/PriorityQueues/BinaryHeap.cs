using System;
using System.Collections.Generic;

namespace PriorityQueues
{
    public sealed class BinaryHeap<TKey, TPriority> : IPriorityQueue<TKey, TPriority>
    {
        internal sealed class BinaryHeapNode : IHeapEntry<TKey, TPriority>
        {
            public TKey Key { get; private set; }
            public TPriority Priority { get; internal set; }
            internal int Index { get; set; }

            internal BinaryHeapNode(TKey key, TPriority priority, int index)
            {
                Key = key;
                Priority = priority;
                Index = index;
            }
        }

        private const int InitialSize = 16;
        private const int Degree = 2;
        private IComparer<TPriority> comparer;

        private BinaryHeapNode[] heap;

        public IHeapEntry<TKey, TPriority> Minimum
        {
            get 
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Binary heap does not contain elements");
                }
                return heap[1]; 
            }
        }

        public int Count { get; private set; }

        public BinaryHeap(IComparer<TPriority> comparer = null)
        {
            if (comparer != null)
            {
                this.comparer = comparer;
            }
            else
            {
                this.comparer = Comparer<TPriority>.Default;
            }
            heap = new BinaryHeapNode[InitialSize];
        }

        public IHeapEntry<TKey, TPriority> Insert(TKey key, TPriority priority)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (priority == null)
            {
                throw new ArgumentNullException("priority");
            }
            if (Count == heap.Length - 1)
            {
                Array.Resize(ref heap, heap.Length * Degree);
            }
            BinaryHeapNode node = new BinaryHeapNode(key, priority, ++Count);
            heap[Count] = node;
            HeapifyUp(node);
            return node;
        }

        public IHeapEntry<TKey, TPriority> RemoveMinimum()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Binary heap is empty!");
            }
            BinaryHeapNode min = heap[1];
            Remove(heap[1]);
            return min;
        }

        public void Increase(IHeapEntry<TKey, TPriority> entry, TPriority priority)
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
                throw new ArgumentException(string.Format("Invalid new priority: {0} (old value: {1})!", priority, entry.Priority));
            }
            BinaryHeapNode node = entry as BinaryHeapNode;
            if (node == null)
            {
                throw new InvalidCastException("Invalid heap entry format!");
            }
            node.Priority = priority;
            HeapifyUp(node);
        }

        public void Remove(IHeapEntry<TKey, TPriority> entry)
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
            if (temp.Index == 0)
            {
                throw new ArgumentException("Heap does not contain this node!");
            }
            if (temp.Index == Count)
            {
                temp.Index = 0;
                heap[Count--] = null;
                return;
            }
            MoveNode(heap[Count], temp.Index);
            heap[Count--] = null;
            HeapifyUp(heap[HeapifyDown(heap[temp.Index])]);
            temp.Index = 0;
        }

        private void HeapifyUp(BinaryHeapNode node)
        {
            BinaryHeapNode parent = Parent(node.Index);
            int to = node.Index;

            while (parent != null && comparer.Compare(parent.Priority, node.Priority) > 0)
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
                return comparer.Compare(heap[temp + 1].Priority, heap[temp].Priority) > 0 ?
                heap[temp] : heap[temp + 1];
            }
        }
    }
}
