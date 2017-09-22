using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;
using System.Linq;

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
            var minimum = heap.Peek;
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void EmptyHeapRemoveMinimumTest()
        {
            var heap = Create();
            heap.Dequeue();
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
            var entry = heap.Enqueue("Item", 0);
            heap.Remove(entry);
            Assert.AreEqual(0, heap.Count);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void HeapRemoveNotContainedTest()
        {
            var heap = Create();
            var entry = heap.Enqueue("Item", 0);
            heap.Remove(entry);
            heap.Remove(entry);
        }

        [TestMethod]
        public void HeapIncreaseTest()
        {
            var heap = Create();
            var entry = heap.Enqueue("Item", 0);
            heap.Increase(entry, -1);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void HeapIncreaseNullEntryTest()
        {
            var heap = Create();
            var entry = heap.Enqueue("Item", 0);
            heap.Increase(null, -1);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void HeapIncreaseInvalidNewpriorityTest()
        {
            var heap = Create();
            var entry = heap.Enqueue("Item", 0);
            heap.Increase(entry, 1);
        }

        [TestMethod]
        public void HeapInsertTest()
        {
            var heap = Create();
            var entry = heap.Enqueue("Item", 0);
            Assert.AreEqual(1, heap.Count);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void HeapInsertNullTest()
        {
            var heap = Create();
            var entry = heap.Enqueue(null, 0);
        }

        [TestMethod]
        public void HeapMultiOperationTest()
        {
            var heap = Create();
            var entry1 = heap.Enqueue("2", 2);
            var entry2 = heap.Enqueue("4", 4);
            var entry3 = heap.Enqueue("6", 6);
            heap.Increase(entry3, 3);
            Assert.AreEqual(entry1, heap.Dequeue());
            Assert.AreEqual(entry3, heap.Peek);
            Assert.AreEqual(2, heap.Count);
        }

        [TestMethod]
        public void EnumerableItemsWithoutRemoveTest()
        {
            var heap = Create();
            var entry1 = heap.Enqueue("a", 1);
            var entry2 = heap.Enqueue("b", 3);
            var entry3 = heap.Enqueue("c", 4);
            var entry4 = heap.Enqueue("d", 2);
            heap.Increase(entry4, 0);
            heap.Increase(entry3, 2);

            // Act
            var items = heap.Select(t => t).ToList();

            // Assert
            Assert.AreEqual(4, items.Count);
            Assert.IsTrue(items.Contains(entry1.Item));
            Assert.IsTrue(items.Contains(entry2.Item));
            Assert.IsTrue(items.Contains(entry3.Item));
            Assert.IsTrue(items.Contains(entry4.Item));
        }

        [TestMethod]
        public void EnumerableItemsWithRemoveTest()
        {
            var heap = Create();
            var entry1 = heap.Enqueue("a", 1);
            var entry2 = heap.Enqueue("b", 3);
            var entry3 = heap.Enqueue("c", 4);
            var entry4 = heap.Enqueue("d", 2);
            heap.Dequeue();
            heap.Remove(entry2);

            // Act
            var items = heap.Select(t => t).ToList();

            // Assert
            Assert.AreEqual(2, items.Count);
            Assert.IsTrue(items.Contains(entry3.Item));
            Assert.IsTrue(items.Contains(entry4.Item));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void NoComparableTest()
        {
            var pq = new BinaryHeap<string, object>();
            pq.Enqueue("", new object());
            pq.Enqueue("", new object());
        }
    }
}
