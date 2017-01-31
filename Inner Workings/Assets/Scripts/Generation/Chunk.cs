using System.Collections.Generic;
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
        m.Clear();

        m.SetVertices(data.points);
        m.SetColors(data.colors);
        m.SetNormals(data.normals);
        m.SetTriangles(data.triangles.ToArray(), 0);

        filter.mesh = m;
    }

    public void Destroy()
    {

    }
}