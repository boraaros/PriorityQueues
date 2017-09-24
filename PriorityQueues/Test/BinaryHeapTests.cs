using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;

namespace Test
{
    [TestClass]
    public class BinaryHeapTests : PriorityQueueTestsBase
    {
        protected override IPriorityQueue<string, TPriority> Create<TPriority>()
        {
            return new BinaryHeap<string, TPriority>();
        }
    }
}
