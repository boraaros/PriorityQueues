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

    public interface IPriorityQueue<TKey, TValue>
    {
        IHeapEntry<TKey, TValue> Minimum { get; }
        int Count { get; }
        IHeapEntry<TKey, TValue> Insert(TKey key, TValue value);
        IHeapEntry<TKey, TValue> RemoveMinimum();
        void Increase(IHeapEntry<TKey, TValue> entry, TValue value);
        void Remove(IHeapEntry<TKey, TValue> entry);
    }
}
