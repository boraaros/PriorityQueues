# PriorityQueues

[![Build status](https://ci.appveyor.com/api/projects/status/g7kufmaqph84fkk5/branch/master?svg=true)](https://ci.appveyor.com/project/boraaros/priorityqueues/branch/master)
[![NuGet](https://img.shields.io/nuget/v/PriorityQueues_boraaros.svg)](https://www.nuget.org/packages/PriorityQueues_boraaros)
[![NuGet](https://img.shields.io/nuget/dt/PriorityQueues_boraaros.svg)](https://github.com/boraaros/PriorityQueues_boraaros)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/boraaros/PriorityQueues/blob/master/LICENSE)

## Source code
* :heavy_check_mark: Clean and simple source code
* :heavy_check_mark: High-speed algorithms
* :heavy_check_mark: LINQ (IEnumerable) support
* :heavy_check_mark: Minimum and maximum priority queue
* :heavy_check_mark: Custom priority comparer allowed
* :heavy_check_mark: Lot of unit tests

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

## Wikipedia links
[Priority queue](https://en.wikipedia.org/wiki/Priority_queue)

[Heap](https://en.wikipedia.org/wiki/Heap_(data_structure))

[Binary heap](https://en.wikipedia.org/wiki/Binary_heap)

[Fibonacci heap](https://en.wikipedia.org/wiki/Fibonacci_heap)
