using System;

namespace PriorityQueues
{
    public interface IHeapEntry<TKey, TPriority>
    {
        TKey Key { get; }
        TPriority Priority { get; }
    }
}
