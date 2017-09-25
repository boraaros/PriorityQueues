# PriorityQueues
* Clean and simple source code
* High-speed algorithms
* LINQ (IEnumerable) support
* Minimum and maximum priority queue
* Custom priority comparer allowed
* Lot of unit tests

## Example
```cs
// Create minimum priority queue
IPriorityQueue<string, int> myPriorityQueue = new BinaryHeap<string, int>(PriorityQueueType.Minimum);
// Insert new items
IPriorityQueueEntry<string> entry1 = myPriorityQueue.Enqueue("TestItem1", 4);
IPriorityQueueEntry<string> entry2 = myPriorityQueue.Enqueue("TestItem2", 3);
IPriorityQueueEntry<string> entry3 = myPriorityQueue.Enqueue("TestItem3", 2);
// Update priority
myPriorityQueue.UpdatePriority(entry2, 1);
// Peek head item and priority
string headItem = myPriorityQueue.Peek; // TestItem2
int headPriority = myPriorityQueue.PeekPriority; // 1
// Dequeue head item
string head = myPriorityQueue.Dequeue(); // TestItem3
// Remove an item or all items
myPriorityQueue.Remove(entry1);
myPriorityQueue.Clear();
int count = myPriorityQueue.Count; // 0
```

## Summary of running times
|Operation   |  Binary      |   Fibonacci   |
-------------|:------------:|:-------------:|
|Peek        |  O(1)        |   O(1)        |
|Enqueue     |  O(log n)    |   O(1)        |    
|Dequeue     |  O(log n)    |   O(log n)    |
|Update      |  O(log n)    |   O(1)        |
|Remove      |  O(log n)    |   O(log n)    |
