using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;

namespace Test
{
    [TestClass]
    public class FibonacciHeapTests : PriorityQueueTestsBase
    {
        protected override IPriorityQueue<string, TPriority> CreateMinimumPriorityQueue<TPriority>()
        {
            return new FibonacciHeap<string, TPriority>(PriorityQueueType.Minimum);
        }

        protected override IPriorityQueue<string, TPriority> CreateMaximumPriorityQueue<TPriority>()
        {
            return new FibonacciHeap<string, TPriority>(PriorityQueueType.Maximum);
        }
    }
}
