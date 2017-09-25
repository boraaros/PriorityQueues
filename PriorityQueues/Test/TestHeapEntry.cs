using PriorityQueues;
using System;

namespace Test
{
    class TestHeapEntry : IHeapEntry<string>
    {
        private readonly string item;

        public TestHeapEntry(string item)
        {
            this.item = item;
        }

        public string Item
        {
            get { return item; }
        }
    }
}
