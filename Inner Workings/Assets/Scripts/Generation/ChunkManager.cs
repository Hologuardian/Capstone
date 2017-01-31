using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    private const int ThreadPoolSize = 4;
    public GameObject chunkPrefab;

    private Dictionary<long, Chunk> chunkMap = new Dictionary<long, Chunk>();
    private List<long> chunkList = new List<long>();
    private Queue<long> deletionPool = new Queue<long>();
    private Queue<ChunkUpdate> chunkUpdateQueue = new Queue<ChunkUpdate>();
    private List<Thread> ThreadPool = new List<Thread>();
    private BlockingQueue<ChunkRequest> generationRequests = new BlockingQueue<ChunkRequest>();
    private BlockingQueue<Chunk> finishedGeneration = new BlockingQueue<Chunk>();
    public static bool KillThread = false;

    /**
     Required to be called in order to create thread pool, uses const definition for number of threads to be generated
    */
    public void Start()
    {
        Chunk.noise = new FastNoise(UnityEngine.Random.Range(0, int.MaxValue));
        StartCoroutine(CheckDeletionQueue());
        StartCoroutine(CheckGenerationQueue());
        //StartCoroutine(CheckUpdateQueue());
        for (int i = 0; i < ThreadPoolSize; i++)
        {
            Thread thread = new Thread(GenerateChunk);
            thread.Start();
            ThreadPool.Add(thread);
        }
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
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

    /**
    Creates a chunk at the requested chunk position, requires a chunk renderer specified to generate the chunk.

    The x and z are the chunk position, which is the absolute position divided by the chunk width.
    */
    public void SetBlock(long x, int y, long z)
    {

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
    Takes two ints and hash them
    */
    public long Hash(int x, int z)
    {
        return ((long)x) + ((long)z) * (((long)int.MaxValue));
    }

    private void GenerateChunk()
    {
        while (true)
        {
            if (KillThread)
                return;
            try
            {
                ChunkRequest request = generationRequests.Pop();
                switch (request.chunkRequestType)
                {
                    case ChunkRequest.RequestType.Generation:
                        {
                            request.chunk.Generate();
                            finishedGeneration.Push(request.chunk);
                            break;
                        }
                    case ChunkRequest.RequestType.Update:
                        {
                            //ChunkUpdate update = request.update;
                            //Chunk chunk;
                            //if (!chunkMap.ContainsKey(update.chunkLocation))
                            //{
                            //    foreach (long i in update.updates)
                            //    {
                            //        ushort id = (ushort)(i & ushort.MaxValue);
                            //        int pos = (int)(i << 16);
                            //        //TODO implement block ID to value/color implementation.
                            //    }
                            //}
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
                Debug.Log(e.Message + " :: " + e.StackTrace);
            }
            
        }
    }

    private void Update()
    {
        
    }

    private IEnumerator CheckGenerationQueue()
    {
        while (true)
        {
            for (int n = 0; n < 5; n++)
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
            for (int n = 0; n < 5; n++)
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
            if (chunkUpdateQueue.Count > 0)
            {
                //generationRequests.run = false;
                //long key = long.MaxValue;
                //key = deletionPool.Dequeue();
                //if (key != long.MaxValue)
                //{
                //    chunkMap[key] = null;
                //    chunkMap.Remove(key);
                //    chunkList.Remove(key);
                //}
            }
            else
            {
                generationRequests.run = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}