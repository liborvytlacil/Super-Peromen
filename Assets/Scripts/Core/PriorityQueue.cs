using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T> {
    private List<T> data = new List<T>();
    
    public void Clear()
    {
        data.Clear();
    }

    public bool IsEmpty()
    {
        return data.Count == 0;
    }

    public int GetCount()
    {
        return data.Count;
    }
    public T Pop()
    {
        T top = data[0];
        T last = data[data.Count - 1];
        data.RemoveAt(data.Count - 1);


        if (data.Count > 0)
        {
            data[0] = last;
            SiftDown();
        }


        return top;
    }

    public T Peek()
    {
        return data[0];
    }

    public void Push(T item)
    {
        data.Add(item);
        SiftUp();
    }
    
    private void SiftUp()
    {
        int childIndex = data.Count - 1; // last item in the heap
        T childValue = data[childIndex]; // preserve its value
        int parentIndex = (childIndex - 1) / 2;
        while (childIndex > parentIndex)
        {
            // if the child is smaller than the parent, swap them and continue sifting in the next iteration
            if (childValue.CompareTo(data[parentIndex]) < 0)
            {
                data[childIndex] = data[parentIndex];
                childIndex = parentIndex;
                parentIndex = (childIndex - 1) / 2;
            } else
            {
                break;
            }
        }
    }

    private void SiftDown()
    {
        int parentIndex = 0; // root
        T parentValue = data[parentIndex]; // preserve the parent data
        var childIndex = 1; // index of the left child;

        while (childIndex < data.Count)
        {
            var rightChildIndex = childIndex + 1; // index of the right child;

            // select the smaller child
            if (rightChildIndex < data.Count && data[rightChildIndex].CompareTo(data[childIndex]) < 0)
            {
                childIndex = rightChildIndex;
            }

            // if the child is smaller than the parent, swap them and continue sifting down the parent in the next iteration
            if (parentValue.CompareTo(data[childIndex]) < 0)
            {
                data[parentIndex] = data[childIndex];
                parentIndex = childIndex;
                childIndex = 2 * parentIndex + 1;
            } else
            {
                break;
            }
        }
    }
}
