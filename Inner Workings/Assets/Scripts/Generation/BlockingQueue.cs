using System.Collections.Generic;
using UnityEngine;

public class BlockingQueue<T>
{
    private object syncLock = new object();
    private object pushLock = new object();
    private Queue<T> queue = new Queue<T>();

    public bool run = true;

    /**
	    Pushes an item onto the queue, thread safe.
	*/
    public void Push(T value)
    {
        lock (pushLock)
        {
            queue.Enqueue(value);
        }
    }

    /**
        Pops an item from the queue, will lock the thread until an item is recieved.
    */
    public T Pop()
    {
        lock (syncLock)
        {
            while (run && !ChunkManager.KillThread)
            {
                if (queue.Count > 0)
                {
                    lock (pushLock)
                    {
                        return queue.Dequeue();
                    }
                }
            }
            return default(T);
        }
    }

    /**
        Checks if queue is empty, returns true if it is.
    */
    public bool Empty()
    {
        return queue.Count <= 0;
    }

    /**
        Clears the queue, not thread safe.
    */
    public void Clear()
    {
        lock (syncLock)
        {
            queue.Clear();
        }
    }

    public int Size()
    {
        lock (syncLock)
        {
            return queue.Count;
        }
    }
}
