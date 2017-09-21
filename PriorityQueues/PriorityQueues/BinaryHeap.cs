using System;
using System.Collections.Generic;

namespace PriorityQueues
{
    public sealed class BinaryHeap<TKey, TValue> : IPriorityQueue<TKey, TValue>
    {
        internal sealed class BinaryHeapNode : IHeapEntry<TKey, TValue>
        {
            public TKey Key { get; private set; }
            public TValue Value { get; internal set; }
            internal int Index { get; set; }

            internal BinaryHeapNode(TKey key, TValue value, int index)
            {
                Key = key;
                Value = value;
                Index = index;
            }
        }

        private const int InitialSize = 16;
        private const int Degree = 2;
        private IComparer<TValue> comparer;

        private BinaryHeapNode[] heap;

        public IHeapEntry<TKey, TValue> Minimum
        {
            get 
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Heap contains no elements");
                }
                return heap[1]; 
            }
        }

        public int Count { get; private set; }

        public BinaryHeap(IComparer<TValue> comparer = null)
        {
            if (comparer != null)
            {
                this.comparer = comparer;
            }
            else
            {
                this.comparer = Comparer<TValue>.Default;
            }
            heap = new BinaryHeapNode[InitialSize];
        }

        public IHeapEntry<TKey, TValue> Insert(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (Count == heap.Length - 1)
            {
                Array.Resize(ref heap, heap.Length * Degree);
            }
            BinaryHeapNode node = new BinaryHeapNode(key, value, ++Count);
            heap[Count] = node;
            HeapifyUp(node);
            return node;
        }

        public IHeapEntry<TKey, TValue> RemoveMinimum()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Heap is empty!");
            }
            BinaryHeapNode min = heap[1];
            Remove(heap[1]);
            return min;
        }

        public void Increase(IHeapEntry<TKey, TValue> entry, TValue value)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (comparer.Compare(value, entry.Value) > 0)
            {
                throw new ArgumentException("Invalid new value!");
            }
            BinaryHeapNode node = (BinaryHeapNode)entry;
            node.Value = value;
            HeapifyUp(node);
        }

        public void Remove(IHeapEntry<TKey, TValue> entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            BinaryHeapNode temp = (BinaryHeapNode)entry;
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

            while (parent != null && comparer.Compare(parent.Value, node.Value) > 0)
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
                return comparer.Compare(heap[temp + 1].Value, heap[temp].Value) > 0 ?
                heap[temp] : heap[temp + 1];
            }
        }
    }
}
