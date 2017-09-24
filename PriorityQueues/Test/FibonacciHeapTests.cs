using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;

namespace Test
{
    [TestClass]
    public class FibonacciHeapTests : PriorityQueueTestsBase
    {
        protected override IPriorityQueue<string, TPriority> Create<TPriority>()
        {
            return new FibonacciHeap<string, TPriority>();
        }
    }
}
