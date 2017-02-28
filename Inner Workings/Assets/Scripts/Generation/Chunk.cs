using System.Collections.Generic;
using System;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [HideInInspector]
    public ChunkData data;
    public ChunkGenerator chunkGenerator;
    public List<ChunkDecorator> chunkDecorators = new List<ChunkDecorator>();
    public List<GameObject> objects = new List<GameObject>();
    public static FastNoise noise;
    public ChunkMesher mesher;

    public void Generate()
    {
        chunkGenerator.generateChunkData(data, noise);
        for (int i = 0; i < chunkDecorators.Count; i++)
        {
            chunkDecorators[i].decorateChunkData(data, noise);
        }
    }

    public void UpdateData(ChunkUpdate update)
    {
        for (int i = 0; i < update.positions.Count; i++)
        {
            long[] pos = update.positions[i];
            int index = -1;
            try
            {
                index = (int)(pos[1] * (Constants.ChunkWidth + 1) * (Constants.ChunkHeight + 1) +
                            pos[3] * (Constants.ChunkHeight + 1) +
                            pos[2]);
                data.values[index] = update.values[i];
            }
            catch(Exception e)
            {
                Debug.Log(e.Message + "\n" + e.StackTrace + "\n" + index + ":" + pos[0] + ":" + pos[1] + ":" + pos[2] + ":" + pos[3] + ":" + update.positions.Count + ":" + update.values.Count);
            }
        }
    }

    public void Mesh()
    {
        mesher.MeshChunk(data);
    }

    public void ThreadInitialize(int x, int z, ChunkMesher mesher)
    {
        this.mesher = mesher;
        data = new ChunkData();
        data.ChunkX = x;
        data.ChunkZ = z;
        gameObject.transform.position = new Vector3(x * Constants.ChunkWidth, 0, z * Constants.ChunkWidth);
    }
        
    public void Initialize()
    {
        objects = new List<GameObject>();


        Mesh m = new Mesh();
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        m.Clear();

        m.SetVertices(data.points);
        m.SetColors(data.colors);
        m.SetNormals(data.normals);
        m.SetTriangles(data.triangles.ToArray(), 0);

        filter.mesh = m;
        collider.sharedMesh = m;
    }

    public void Destroy()
    {

    }
}