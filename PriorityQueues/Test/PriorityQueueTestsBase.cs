using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueues;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestClass]
    public abstract class PriorityQueueTestsBase
    {
        protected abstract IPriorityQueue<string, TPriority> CreateMinimumPriorityQueue<TPriority>();
        protected abstract IPriorityQueue<string, TPriority> CreateMaximumPriorityQueue<TPriority>();


        [TestMethod]
        public void Empty_priority_queue_count_is_zero()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            Assert.AreEqual(0, priorityQueue.Count);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void Empty_priority_queue_peek_throws_exception()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            string item = priorityQueue.Peek;
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void Empty_priority_queue_peek_priority_throws_exception()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            int priority = priorityQueue.PeekPriority;
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void Empty_priority_queue_dequeue_throws_exception()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Dequeue();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Remove_from_priority_queue_throws_exception_if_parameter_is_null()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Remove(null);
        }

        [TestMethod]
        public void Remove_single_item_from_priority_queue()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 0);
            priorityQueue.Remove(entry);
            Assert.AreEqual(0, priorityQueue.Count);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Remove_item_from_priority_queue_if_already_removed()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 0);
            priorityQueue.Remove(entry);
            priorityQueue.Remove(entry);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Enqueue_throws_exception_if_item_is_null()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Enqueue(null, 1);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Enqueue_throws_exception_if_priority_is_null()
        {
            IPriorityQueue<string, string> priorityQueue = CreateMinimumPriorityQueue<string>();
            priorityQueue.Enqueue("Test", null);
        }

        [TestMethod]
        public void Enqueue_single_item_and_peek()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Enqueue("Test", 1);
            Assert.AreEqual(1, priorityQueue.Count);
            Assert.AreEqual("Test", priorityQueue.Peek);
            Assert.AreEqual(1, priorityQueue.PeekPriority);
        }

        [TestMethod]
        public void Enqueue_single_item_with_negative_priority()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Enqueue("Test", -1);
            Assert.AreEqual(-1, priorityQueue.PeekPriority);
        }

        [TestMethod]
        public void Enqueue_item_several_times_with_different_priority()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            string item = "Test";
            priorityQueue.Enqueue(item, 0);
            priorityQueue.Enqueue(item, 1);
            Assert.AreEqual(2, priorityQueue.Count);
        }

        [TestMethod]
        public void Enqueue_different_items_with_same_priority()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Enqueue("Test1", 1);
            priorityQueue.Enqueue("Test2", 1);
            Assert.AreEqual(2, priorityQueue.Count);
        }

        [TestMethod]
        public void Enqueue_if_head_item_changed()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Enqueue("Test1", 1);
            priorityQueue.Enqueue("Test2", 0);
            Assert.AreEqual("Test2", priorityQueue.Peek);
            Assert.AreEqual(0, priorityQueue.PeekPriority);
        }

        [TestMethod]
        public void Increase_priority_of_single_item()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 2);
            priorityQueue.UpdatePriority(entry, 1);
            Assert.AreEqual(1, priorityQueue.PeekPriority);
        }

        [TestMethod]
        public void Decrease_priority_of_single_item()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 1);
            priorityQueue.UpdatePriority(entry, 2);
            Assert.AreEqual(2, priorityQueue.PeekPriority);
        }

        [TestMethod]
        public void Update_but_new_priority_equals_old_priority()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 1);
            priorityQueue.UpdatePriority(entry, 1);
            Assert.AreEqual(1, priorityQueue.PeekPriority);
        }

        [TestMethod]
        public void Update_priority_if_head_item_changed()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry1 = priorityQueue.Enqueue("Test1", 1);
            IPriorityQueueEntry<string> entry2 = priorityQueue.Enqueue("Test2", 2);
            priorityQueue.UpdatePriority(entry2, 0);
            Assert.AreEqual("Test2", priorityQueue.Peek);
            Assert.AreEqual(0, priorityQueue.PeekPriority);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Update_throws_exception_if_entry_is_null()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Item", 1);
            priorityQueue.UpdatePriority(null, 0);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Update_throws_exception_if_priority_is_null()
        {
            IPriorityQueue<string, string> priorityQueue = CreateMinimumPriorityQueue<string>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Item", "1");
            priorityQueue.UpdatePriority(entry, null);
        }

        [TestMethod]
        public void Enumerate_items_if_priority_queue_is_empty()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            Assert.IsFalse(priorityQueue.Any());
        }

        [TestMethod]
        public void Enumerate_items_if_only_enqueue_called()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry1 = priorityQueue.Enqueue("Test1", 1);
            IPriorityQueueEntry<string> entry2 = priorityQueue.Enqueue("Test2", 2);
            IList<string> items = priorityQueue.Select(t => t).ToList();
            Assert.AreEqual(2, items.Count);
            Assert.IsTrue(items.Contains(entry1.Item));
            Assert.IsTrue(items.Contains(entry2.Item));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Enqueue_two_item_with_non_comparable_priority()
        {
            IPriorityQueue<string, object> priorityQueue = CreateMinimumPriorityQueue<object>();
            priorityQueue.Enqueue("Test1", new object());
            priorityQueue.Enqueue("Test2", new object());
        }

        [TestMethod]
        public void Clear_if_priority_queue_is_already_empty()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Clear();
            Assert.AreEqual(0, priorityQueue.Count);
        }

        [TestMethod]
        public void Clear_priority_queue_count_is_zero()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 1);
            priorityQueue.Clear();
            Assert.AreEqual(0, priorityQueue.Count);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Update_throws_exception_after_clear()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 1);
            priorityQueue.Clear();
            priorityQueue.UpdatePriority(entry, 2);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void Peek_throws_exception_after_clear()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 1);
            priorityQueue.Clear();
            string item = priorityQueue.Peek;
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void Peek_priority_throws_exception_after_clear()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry = priorityQueue.Enqueue("Test", 1);
            priorityQueue.Clear();
            int priority = priorityQueue.PeekPriority;
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Update_throws_exception_if_parameter_comes_from_another_queue()
        {
            IPriorityQueue<string, int> priorityQueue1 = CreateMinimumPriorityQueue<int>();
            IPriorityQueue<string, int> priorityQueue2 = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry1 = priorityQueue1.Enqueue("Test", 1);
            IPriorityQueueEntry<string> entry2 = priorityQueue2.Enqueue("Test", 1);
            priorityQueue1.UpdatePriority(entry2, 0);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Remove_throws_exception_if_parameter_comes_from_another_queue()
        {
            IPriorityQueue<string, int> priorityQueue1 = CreateMinimumPriorityQueue<int>();
            IPriorityQueue<string, int> priorityQueue2 = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry1 = priorityQueue1.Enqueue("Test", 1);
            IPriorityQueueEntry<string> entry2 = priorityQueue2.Enqueue("Test", 1);
            priorityQueue1.Remove(entry2);
        }

        [ExpectedException(typeof(InvalidCastException))]
        [TestMethod]
        public void Update_throws_exception_if_entry_is_invalid_type()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Enqueue("Test", 1);
            IPriorityQueueEntry<string> entry = new TestPriorityQueueEntry("Test");
            priorityQueue.UpdatePriority(entry, 0);
        }

        [ExpectedException(typeof(InvalidCastException))]
        [TestMethod]
        public void Remove_throws_exception_if_entry_is_invalid_type()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            priorityQueue.Enqueue("Test", 1);
            IPriorityQueueEntry<string> entry = new TestPriorityQueueEntry("Test");
            priorityQueue.Remove(entry);
        }

        [TestMethod]
        public void Complex_minimum_priority_queue_test_with_multiple_operation()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMinimumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry1 = priorityQueue.Enqueue("a", 1);
            IPriorityQueueEntry<string> entry2 = priorityQueue.Enqueue("b", 2);
            IPriorityQueueEntry<string> entry3 = priorityQueue.Enqueue("c", 3);
            IPriorityQueueEntry<string> entry4 = priorityQueue.Enqueue("d", 4);
            priorityQueue.UpdatePriority(entry1, 5);
            priorityQueue.UpdatePriority(entry2, 5);
            priorityQueue.UpdatePriority(entry1, 7);
            priorityQueue.UpdatePriority(entry2, 1);
            priorityQueue.UpdatePriority(entry4, 4);
            priorityQueue.UpdatePriority(entry3, 6);
            priorityQueue.UpdatePriority(entry1, 10);

            Assert.AreEqual(1, priorityQueue.PeekPriority);
            Assert.AreEqual("b", priorityQueue.Dequeue());

            Assert.AreEqual(4, priorityQueue.PeekPriority);
            Assert.AreEqual("d", priorityQueue.Dequeue());

            Assert.AreEqual(6, priorityQueue.PeekPriority);
            Assert.AreEqual("c", priorityQueue.Dequeue());

            Assert.AreEqual(10, priorityQueue.PeekPriority);
            Assert.AreEqual("a", priorityQueue.Dequeue());

            Assert.AreEqual(0, priorityQueue.Count);
        }

        [TestMethod]
        public void Complex_maximum_priority_queue_test_with_multiple_operation()
        {
            IPriorityQueue<string, int> priorityQueue = CreateMaximumPriorityQueue<int>();
            IPriorityQueueEntry<string> entry1 = priorityQueue.Enqueue("a", -1);
            IPriorityQueueEntry<string> entry2 = priorityQueue.Enqueue("b", -2);
            IPriorityQueueEntry<string> entry3 = priorityQueue.Enqueue("c", -3);
            IPriorityQueueEntry<string> entry4 = priorityQueue.Enqueue("d", -4);
            priorityQueue.UpdatePriority(entry1, -5);
            priorityQueue.UpdatePriority(entry2, -5);
            priorityQueue.UpdatePriority(entry1, -7);
            priorityQueue.UpdatePriority(entry2, -1);
            priorityQueue.UpdatePriority(entry4, -4);
            priorityQueue.UpdatePriority(entry3, -6);
            priorityQueue.UpdatePriority(entry1, -10);

            Assert.AreEqual(-1, priorityQueue.PeekPriority);
            Assert.AreEqual("b", priorityQueue.Dequeue());

            Assert.AreEqual(-4, priorityQueue.PeekPriority);
            Assert.AreEqual("d", priorityQueue.Dequeue());

            Assert.AreEqual(-6, priorityQueue.PeekPriority);
            Assert.AreEqual("c", priorityQueue.Dequeue());

            Assert.AreEqual(-10, priorityQueue.PeekPriority);
            Assert.AreEqual("a", priorityQueue.Dequeue());

            Assert.AreEqual(0, priorityQueue.Count);
        }
    }
}
