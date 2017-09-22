using System;
using System.Collections.Generic;

namespace PriorityQueues
{
    //Operation   ||  Binary      |   Fibonacci   |
    //============||==============|===============|
    //Peek        ||  O(1)        |   O(1)        |
    //Enqueue     ||  O(log n)    |   O(1)        |    
    //Dequeue     ||  O(log n)    |   O(log n)    |
    //Increase    ||  O(log n)    |   O(1)        |
    //Remove      ||  O(log n)    |   O(log n)    |

    public interface IPriorityQueue<TItem, TPriority> : IEnumerable<TItem>
    {
        int Count { get; }
        IHeapEntry<TItem, TPriority> Peek { get; }     
        IHeapEntry<TItem, TPriority> Enqueue(TItem item, TPriority priority);
        IHeapEntry<TItem, TPriority> Dequeue();
        void Increase(IHeapEntry<TItem, TPriority> entry, TPriority priority);
        void Remove(IHeapEntry<TItem, TPriority> entry);
    }
}
