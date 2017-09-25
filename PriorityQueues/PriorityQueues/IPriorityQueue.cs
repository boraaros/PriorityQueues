using System;
using System.Collections.Generic;

namespace PriorityQueues
{
    //Operation   ||  Binary      |   Fibonacci   |
    //============||==============|===============|
    //Peek        ||  O(1)        |   O(1)        |
    //Enqueue     ||  O(log n)    |   O(1)        |    
    //Dequeue     ||  O(log n)    |   O(log n)    |
    //Update      ||  O(log n)    |   O(1)        |
    //Remove      ||  O(log n)    |   O(log n)    |

    public interface IPriorityQueue<TItem, TPriority> : IEnumerable<TItem>
    {
        int Count { get; }
        TItem Peek { get; }
        TPriority PeekPriority { get; }
        IPriorityQueueEntry<TItem> Enqueue(TItem item, TPriority priority);
        TItem Dequeue();
        void UpdatePriority(IPriorityQueueEntry<TItem> entry, TPriority priority);
        void Remove(IPriorityQueueEntry<TItem> entry);
        void Clear();
    }
}
