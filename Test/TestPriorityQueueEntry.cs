using PriorityQueues;
using System;

namespace Test
{
    class TestPriorityQueueEntry : IPriorityQueueEntry<string>
    {
        private readonly string item;

        public TestPriorityQueueEntry(string item)
        {
            this.item = item;
        }

        public string Item
        {
            get { return item; }
        }
    }
}
