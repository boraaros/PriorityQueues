using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;

namespace Test
{
    [TestClass]
    public abstract class PriorityQueueTestsBase
    {
        protected abstract IPriorityQueue<string, int> Create();

        [TestMethod]
        public void EmptyHeapCountTest()
        {
            var heap = Create();
            Assert.AreEqual(0, heap.Count);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void EmptyHeapMinimumTest()
        {
            var heap = Create();
            var minimum = heap.Minimum;
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void EmptyHeapRemoveMinimumTest()
        {
            var heap = Create();
            heap.RemoveMinimum();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void HeapRemoveNullTest()
        {
            var heap = Create();
            heap.Remove(null);
        }

        [TestMethod]
        public void HeapRemoveTest()
        {
            var heap = Create();
            var entry = heap.Insert("Key", 0);
            heap.Remove(entry);
            Assert.AreEqual(0, heap.Count);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void HeapRemoveNotContainedTest()
        {
            var heap = Create();
            var entry = heap.Insert("Key", 0);
            heap.Remove(entry);
            heap.Remove(entry);
        }

        [TestMethod]
        public void HeapIncreaseTest()
        {
            var heap = Create();
            var entry = heap.Insert("Key", 0);
            heap.Increase(entry, -1);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void HeapIncreaseNullEntryTest()
        {
            var heap = Create();
            var entry = heap.Insert("Key", 0);
            heap.Increase(null, -1);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void HeapIncreaseInvalidNewValueTest()
        {
            var heap = Create();
            var entry = heap.Insert("Key", 0);
            heap.Increase(entry, 1);
        }

        [TestMethod]
        public void HeapInsertTest()
        {
            var heap = Create();
            var entry = heap.Insert("Key", 0);
            Assert.AreEqual(1, heap.Count);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void HeapInsertNullTest()
        {
            var heap = Create();
            var entry = heap.Insert(null, 0);
        }

        [TestMethod]
        public void HeapMultiOperationTest()
        {
            var heap = Create();
            var entry1 = heap.Insert("2", 2);
            var entry2 = heap.Insert("4", 4);
            var entry3 = heap.Insert("6", 6);
            heap.Increase(entry3, 3);
            Assert.AreEqual(entry1, heap.RemoveMinimum());
            Assert.AreEqual(entry3, heap.Minimum);
            Assert.AreEqual(2, heap.Count);
        }
    }
}
