using System;

namespace PriorityQueues
{
    public interface IHeapEntry<TItem, TPriority>
    {
        TItem Item { get; }
        TPriority Priority { get; }
    }
}
