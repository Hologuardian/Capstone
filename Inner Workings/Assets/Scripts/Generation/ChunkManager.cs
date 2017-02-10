using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChunkManager : MonoBehaviour
{
    private const int ThreadPoolSize = 6;
    private const int CheckNum = 4;
    private const int width = 20;
    public GameObject chunkPrefab;
    public Text textBox;

    private Dictionary<long, bool> generatedChunk = new Dictionary<long, bool>();
    private BlockingDictionary<long, Chunk> chunkMap = new BlockingDictionary<long, Chunk>();
    private BlockingDictionary<long, ChunkUpdate> updateMap = new BlockingDictionary<long, ChunkUpdate>();
    private Dictionary<long, object> lockMap = new Dictionary<long, object>();
    private List<long> chunkList = new List<long>();
    private Queue<long> deletionPool = new Queue<long>();
    private Queue<ChunkUpdate> chunkUpdateQueue = new Queue<ChunkUpdate>();
    private List<Thread> ThreadPool = new List<Thread>();
    private BlockingQueue<ChunkRequest> generationRequests = new BlockingQueue<ChunkRequest>();
    private BlockingQueue<Chunk> finishedGeneration = new BlockingQueue<Chunk>();
    private BlockingQueue<Chunk> finishedUpdate = new BlockingQueue<Chunk>();
    public static bool KillThread = false;

    /**
     Required to be called in order to create thread pool, uses const definition for number of threads to be generated
    */
    public void Start()
    {
        Chunk.noise = new FastNoise(1234);//UnityEngine.Random.Range(0, int.MaxValue));
        StartCoroutine(CheckDeletionQueue());
        StartCoroutine(CheckGenerationQueue());
        StartCoroutine(CheckUpdateQueue());
        Thread thread = new Thread(CheckUpdateMap);
        thread.Start();
        for (int i = 0; i < ThreadPoolSize; i++)
        {
            thread = new Thread(GenerateChunk);
            thread.Start();
            ThreadPool.Add(thread);
        }
        for (int i = 2; i < 2 + width; i++)
        {
            for (int j = 2; j < 2 + width; j++)
            {
                RequestChunk(i, j, new MarchingChunkMesher());
            }
        }
    }

    public void OnDisable()
    {
        StopAllCoroutines();

        KillThread = true;
    }

    /**
        Destroys all currently generated chunks
        Attempts to clear generation request queue, but cannot garuntee chunks in mid-generation will be stopped.
    */
    public void Clear()
    {
        foreach (long c in chunkList)
        {
            chunkMap[c].Destroy();
        }
        chunkList.Clear();
        chunkMap.Clear();
    }

    /**
	Creates a chunk at the requested chunk position

	The x and z are the chunk position, which is the absolute position divided by the chunk width.
	*/
    public void RequestChunk(int x, int z, ChunkMesher mesher)
    {
        Chunk chunk = Instantiate(chunkPrefab).GetComponent<Chunk>();
        chunk.ThreadInitialize(x, z, mesher);
        generationRequests.Push(new ChunkRequest(ChunkRequest.RequestType.Generation, chunk));
    }

    /**
    Requests a specific chunk to be removed, this is not currently a threaded operation.

    The x and z are the chunk position, which is the absolute position divided by the chunk width.
    */
    public void RemoveChunk(int x, int z)
    {
        generationRequests.run = false;
        deletionPool.Enqueue(Hash(x, z));
        return;
    }

    public void SetBlock(long x, int y, long z, uint value)
    {
        long hash = HashBlock(x, z);
        if (!lockMap.ContainsKey(hash))
            lockMap.Add(hash, new object());
        lock (lockMap[hash])
        {
            if (updateMap.ContainsKey(hash))
            {
                int xVal = (int)(x % Constants.ChunkWidth);
                int zVal = (int)(z % Constants.ChunkWidth);
                updateMap[hash].positions.Add(new long[] { hash, xVal, y, zVal });
                updateMap[hash].values.Add(value);
                if (xVal == 0)
                {
                    hash = HashBlock(x - 1, z);
                    if (!lockMap.ContainsKey(hash))
                        lockMap.Add(hash, new object());
                    lock (lockMap[hash])
                    {
                        if (updateMap.ContainsKey(hash))
                        {
                            xVal = Constants.ChunkWidth;
                            zVal = (int)(z % Constants.ChunkWidth);
                            updateMap[hash].positions.Add(new long[] { hash, xVal, y, zVal });
                            updateMap[hash].values.Add(value);
                        }
                        else
                        {
                            ChunkUpdate update = new ChunkUpdate();
                            update.positions.Add(new long[] { hash, Constants.ChunkWidth, y, z % Constants.ChunkWidth });
                            update.values.Add(value);
                            updateMap[hash] = update;
                        }
                    }
                }
                if (zVal == 0)
                {
                    hash = HashBlock(x, z - 1);
                    if (!lockMap.ContainsKey(hash))
                        lockMap.Add(hash, new object());
                    lock (lockMap[hash])
                    {
                        if (updateMap.ContainsKey(hash))
                        {
                            xVal = (int)(x % Constants.ChunkWidth);
                            zVal = Constants.ChunkWidth;
                            updateMap[hash].positions.Add(new long[] { hash, xVal, y, zVal });
                            updateMap[hash].values.Add(value);
                        }
                        else
                        {
                            ChunkUpdate update = new ChunkUpdate();
                            update.positions.Add(new long[] { hash, x % Constants.ChunkWidth, y, Constants.ChunkWidth });
                            update.values.Add(value);
                            updateMap[hash] = update;
                        }
                    }
                }
            }
            else
            {
                ChunkUpdate update = new ChunkUpdate();
                update.positions.Add(new long[] { hash, x % Constants.ChunkWidth, y, z % Constants.ChunkWidth });
                update.values.Add(value);
                updateMap[hash] = update;
            }
        }
    }

    /**
    Changes the generation seed of the requested chunks

    This will cause splits in the terrain to form
    */
    public void SetSeed(int seed)
    {
        Chunk.noise.SetSeed(seed);
    }

    /**
    Takes a chunk pointer and hashes it's position
    */
    public long Hash(Chunk chunk)
    {
        return ((long)chunk.data.ChunkX) + ((long)chunk.data.ChunkZ * ((long)int.MaxValue));
    }

    /**
    Takes block position and gives a chunk hash
    */
    public long HashBlock(long x, long z)
    {
        return (x / Constants.ChunkWidth) + (z / Constants.ChunkWidth) * (((long)int.MaxValue));
    }

    /**
    Takes block position and gives a chunk hash
    */
    public long HashBlock(int x, int z)
    {
        return ((long)x / Constants.ChunkWidth) + ((long)z / Constants.ChunkWidth) * (((long)int.MaxValue));
    }

    /**
    Takes two ints and hash them
    */
    public long Hash(int x, int z)
    {
        return ((long)x) + ((long)z) * (((long)int.MaxValue));
    }

    //Repeating Thread Method.
    private void GenerateChunk()
    {
        while (true)
        {
            if (KillThread)
                return;
            try
            {
                ChunkRequest request = generationRequests.Pop();
                if (request == null)
                    return;
                switch (request.chunkRequestType)
                {
                    case ChunkRequest.RequestType.Generation:
                        {
                            long hash = Hash(request.chunk);
                            request.chunk.Generate();
                            request.chunk.Mesh();
                            if (!lockMap.ContainsKey(hash))
                                lockMap.Add(hash, new object());
                            lock (lockMap[hash])
                            {
                                if (updateMap.ContainsKey(Hash(request.chunk)))
                                {
                                    ChunkUpdate update = updateMap[hash];
                                    request.chunk.UpdateData(update);
                                    //updateMap.Remove(hash);
                                    request.chunk.Mesh();
                                }

                                finishedGeneration.Push(request.chunk);
                            }
                            break;
                        }
                    case ChunkRequest.RequestType.Update:
                        {
                            ChunkUpdate update = request.update;
                            long hash = update.positions[0][0];
                            if (!lockMap.ContainsKey(hash))
                                lockMap.Add(hash, new object());
                            lock (lockMap[hash])
                            {
                                if (chunkMap.ContainsKey(hash))
                                {
                                    chunkMap[hash].UpdateData(update);
                                    chunkMap[hash].Mesh();
                                    finishedUpdate.Push(chunkMap[hash]);
                                }
                                else if (generatedChunk.ContainsKey(hash))
                                {
                                    if (updateMap.ContainsKey(hash))
                                    {
                                        updateMap[hash].positions.AddRange(update.positions);
                                        updateMap[hash].values.AddRange(update.values);
                                    }
                                    updateMap.Add(hash, update);
                                }
                                else
                                {
                                    if (updateMap.ContainsKey(hash))
                                    {
                                        try
                                        {
                                            updateMap[hash].positions.AddRange(update.positions);
                                            updateMap[hash].values.AddRange(update.values);
                                        }
                                        catch (Exception e)
                                        {
                                            Debug.Log(e.Message + "\n" + e.StackTrace + "\n" + (updateMap[hash] != null) + "::" + (update.positions != null) + "::" + (update.values != null));
                                        }
                                    }
                                    else
                                    {
                                        updateMap.Add(hash, update);
                                    }
                                }
                            }

                            break;
                        }
                    case ChunkRequest.RequestType.Load:
                        {
                            break;
                        }
                    case ChunkRequest.RequestType.Save:
                        {
                            break;
                        }
                    case ChunkRequest.RequestType.Deletion:
                        {
                            break;
                        }
                }
            }
            catch(Exception e)
            {
                Debug.Log(e.Message + " :: " + e.StackTrace + "::" + e.Data);
            }
            
        }
    }

    private void Update()
    {
        textBox.text = String.Format("Thread Queue: {0}\n", generationRequests.Size())
        + String.Format("Generated Chunks: {0}\n", chunkList.Count)
        + String.Format("Chunk Updates: {0}\n", chunkUpdateQueue.Count)
        + String.Format("Update Map: {0}\n", updateMap.Count)
        + String.Format("Finished Gen: {0}\n", finishedGeneration.Size())
        + String.Format("Finished Update: {0}\n", finishedUpdate.Size());
    }

    private IEnumerator CheckGenerationQueue()
    {
        while (true)
        {
            for (int n = 0; n < CheckNum; n++)
            {
            if (!finishedGeneration.Empty())
                {
                    Chunk chunk = finishedGeneration.Pop();
                    chunk.Initialize();
                    long key = Hash(chunk);
                    chunkList.Add(key);
                    if (!chunkMap.ContainsKey(key))
                        chunkMap[key] = chunk;
                    else
                        Debug.Log("ERROR: Chunk Hash collision at: " + key + " :: " + chunk.data.ChunkX + " " + chunk.data.ChunkZ);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CheckDeletionQueue()
    {
        while (true)
        {
            for (int n = 0; n < CheckNum; n++)
            {
                if (deletionPool.Count > 0)
                {
                    generationRequests.run = false;
                    long key = long.MaxValue;
                    key = deletionPool.Dequeue();
                    if (key != long.MaxValue)
                    {
                        chunkMap[key] = null;
                        chunkMap.Remove(key);
                        chunkList.Remove(key);
                    }
                }
                else
                {
                    generationRequests.run = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CheckUpdateQueue()
    {
        while (true)
        {
            for (int n = 0; n < CheckNum; n++)
            {
                if (!finishedUpdate.Empty())
                {
                    Chunk chunk = finishedUpdate.Pop();
                    chunk.Initialize();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void CheckUpdateMap()
    {
        while (true)
        {
            if (KillThread)
                return;
            if (updateMap.Count > 0 && generationRequests.Size() == 0)
            {
                List<long> delete = new List<long>();
                foreach(long key in updateMap.Keys)
                {
                    generationRequests.Push(new ChunkRequest(ChunkRequest.RequestType.Update, updateMap[key]));
                    delete.Add(key);
                }
                foreach(long key in delete)
                {
                    lock(lockMap[key])
                    {
                        updateMap.Remove(key);
                    }
                }
                delete.Clear();
            }
        }
    }
}