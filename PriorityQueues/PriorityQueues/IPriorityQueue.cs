using System;

namespace PriorityQueues
{
    //Operation   ||  Binary      |   Fibonacci   |
    //============||==============|===============|
    //Insert      ||  O(log n)    |   O(1)        |
    //Minimum     ||  O(1)        |   O(1)        |
    //RemoveMin   ||  O(log n)    |   O(log n)    |
    //Increase    ||  O(log n)    |   O(1)        |
    //Remove      ||  O(log n)    |   O(log n)    |

    public interface IPriorityQueue<TItem, TPriority>
    {
        IHeapEntry<TItem, TPriority> Minimum { get; }
        int Count { get; }
        IHeapEntry<TItem, TPriority> Insert(TItem item, TPriority priority);
        IHeapEntry<TItem, TPriority> RemoveMinimum();
        void Increase(IHeapEntry<TItem, TPriority> entry, TPriority priority);
        void Remove(IHeapEntry<TItem, TPriority> entry);
    }
}
