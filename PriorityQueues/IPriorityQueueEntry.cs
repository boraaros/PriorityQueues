using System;

namespace PriorityQueues
{
    public interface IPriorityQueueEntry<TItem>
    {
        TItem Item { get; }
    }
}
