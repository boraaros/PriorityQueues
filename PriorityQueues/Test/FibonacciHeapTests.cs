using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;

namespace Test
{
    [TestClass]
    public class FibonacciHeapTests : PriorityQueueTestsBase
    {
        protected override IPriorityQueue<string, int> Create()
        {
            return new FibonacciHeap<string, int>();
        }
    }
}
