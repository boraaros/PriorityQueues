using System;

namespace PriorityQueues
{
    public interface IHeapEntry<TKey, TValue>
    {
        TKey Key { get; }
        TValue Value { get; }
    }
}
