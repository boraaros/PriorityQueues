using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;

namespace Test
{
    [TestClass]
    public class BinaryHeapTests : PriorityQueueTestsBase
    {

        protected override IPriorityQueue<string, int> Create()
        {
            return new BinaryHeap<string, int>();
        }
    }
}
