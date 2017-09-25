using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;

namespace Test
{
    [TestClass]
    public class BinaryHeapTests : PriorityQueueTestsBase
    {
        protected override IPriorityQueue<string, TPriority> CreateMinimumPriorityQueue<TPriority>()
        {
            return new BinaryHeap<string, TPriority>(PriorityQueueType.Minimum);
        }

        protected override IPriorityQueue<string, TPriority> CreateMaximumPriorityQueue<TPriority>()
        {
            return new BinaryHeap<string, TPriority>(PriorityQueueType.Maximum);
        }
    }
}
