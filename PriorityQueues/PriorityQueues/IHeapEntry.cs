using System;

namespace PriorityQueues
{
    public interface IHeapEntry<TItem>
    {
        TItem Item { get; }
    }
}
