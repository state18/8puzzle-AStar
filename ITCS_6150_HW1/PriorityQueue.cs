using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// MinHeap-based Priority Queue
/// </summary>
/// <typeparam name="T">Item Type</typeparam>
class PriorityQueue<T> {
    private static Dictionary<int, int> inCounter = new Dictionary<int, int>();
    private static Dictionary<int, int> outCounter = new Dictionary<int, int>();
    private Item[] items;

    private int size;
    private int capacity;

    public PriorityQueue(int capacity = 10, bool hasUniqueKeys = true) {
        this.capacity = capacity;
        items = new Item[capacity];
    }

    public static bool TestPriorityQueue(int testSize = 100) {

        Random rnd = new Random();
        var pq = new PriorityQueue<int>(1);
        var testInput = new int[testSize];

        // Enqueue
        for (int i = 0; i < testSize; i++) {
            testInput[i] = rnd.Next(testSize);
            pq.Enqueue(testInput[i], testInput[i]);
        }

        testInput = testInput.Distinct().ToArray();

        // Dequeue
        var testOutput = new int[testInput.Length];
        int numDequeued = 0;
        while (!pq.IsEmpty()) {
            testOutput[numDequeued] = pq.Dequeue();
            numDequeued++;
        }

        Array.Sort(testInput);

        bool isPass = testInput.SequenceEqual(testOutput);
        return isPass;

    }

    public bool IsEmpty() {
        return size == 0;
    }

    public bool Enqueue(T data, int priority) {

        // Update priority if item already exists in queue and new priority value is smaller.
        bool alreadyExists = false;

        int foundIndex = -1;
        for (int i = 0; i < size; i++) {
            if (items[i].Data.Equals(data)) {
                foundIndex = i;
                break;
            }
        }

        if (foundIndex >= 0) {
            alreadyExists = true;
            Item foundItem = items[foundIndex];
            if (foundItem.Priority > priority) {
                items[foundIndex] = new Item(data, priority);

                SiftUp(foundIndex);

                inCounter[foundItem.Priority]--;

                if (inCounter.ContainsKey(priority))
                    inCounter[priority]++;
                else
                    inCounter[priority] = 1;
            }

        } else {
            // Item does not already exist in queue, so create and add it.
            Item newItem = new Item(data, priority);
            items[size] = newItem;

            // Heapify up. 
            SiftUp(size);
            size++;

            // Grow if at max capacity.
            if (size == capacity) {
                capacity *= 2;
                Item[] tmp = items;
                items = new Item[capacity];
                for (int i = 0; i < size; i++) {
                    items[i] = tmp[i];
                }
            }
            if (inCounter.ContainsKey(priority))
                inCounter[priority]++;
            else
                inCounter[priority] = 1;
        }



        return alreadyExists;

    }

    public T Dequeue() {

        Item popped = items[0];
        Item newRoot = items[size - 1];
        items[0] = newRoot;
        items[size - 1] = null;
        size--;

        SiftDown(0);

        if (outCounter.ContainsKey(popped.Priority))
            outCounter[popped.Priority]++;
        else
            outCounter[popped.Priority] = 1;

        return popped.Data;
    }

    private void SiftDown(int startIndex) {

        Item siftingItem = items[startIndex];

        int currIndex = startIndex + 1;
        bool continueSifting = true;
        while (currIndex <= size / 2 && continueSifting) {
            int leftChildIndex = currIndex * 2;
            int rightChildIndex = currIndex * 2 + 1;

            int smallerChild = items[rightChildIndex - 1] == null ||
                items[leftChildIndex - 1].Priority < items[rightChildIndex - 1].Priority ? leftChildIndex : rightChildIndex;

            if (items[smallerChild - 1].Priority < siftingItem.Priority) {
                items[currIndex - 1] = items[smallerChild - 1];
                items[smallerChild - 1] = siftingItem;
            } else
                continueSifting = false;

            currIndex = smallerChild;
        }
    }

    private void SiftUp(int startIndex) {

        Item siftingItem = items[startIndex];

        int currIndex = startIndex + 1;
        bool continueSifting = true;
        while (currIndex > 1 && continueSifting) {
            int parentIndex = currIndex / 2;
            if (items[parentIndex - 1].Priority > siftingItem.Priority) {
                items[currIndex - 1] = items[parentIndex - 1];
                items[parentIndex - 1] = siftingItem;
            } else
                continueSifting = false;

            currIndex = parentIndex;
        }
    }


    class Item {
        public T Data { get; set; }
        public int Priority { get; set; }

        public Item(T data, int priority) {
            Data = data;
            Priority = priority;
        }
    }

}
