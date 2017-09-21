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

    public interface IPriorityQueue<TKey, TPriority>
    {
        IHeapEntry<TKey, TPriority> Minimum { get; }
        int Count { get; }
        IHeapEntry<TKey, TPriority> Insert(TKey key, TPriority priority);
        IHeapEntry<TKey, TPriority> RemoveMinimum();
        void Increase(IHeapEntry<TKey, TPriority> entry, TPriority priority);
        void Remove(IHeapEntry<TKey, TPriority> entry);
    }
}
