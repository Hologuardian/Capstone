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
    private ReflectionProbe probe;

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

    public void ThreadInitialize(int x, int z, ChunkMesher mesher, Vector2 offset)
    {
        this.mesher = mesher;
        data = new ChunkData();
        data.ChunkX = x;
        data.ChunkZ = z;
        gameObject.transform.position = new Vector3(x * Constants.ChunkWidth + offset.x, 0, z * Constants.ChunkWidth + offset.y);
    }
        
    public void Initialize()
    {
        objects = new List<GameObject>();

        foreach (Interactable interactable in data.interactables)
        {
            interactable.GenerateInteractionSphere(gameObject.transform);
        }

        try
        {
            Mesh m = new Mesh();
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            MeshCollider collider = gameObject.GetComponent<MeshCollider>();
            m.Clear();
            m.SetVertices(data.points);
            m.SetColors(data.colors);
            m.SetNormals(data.normals);
            m.SetTriangles(data.triangles, 0);

            filter.mesh = m;
            collider.sharedMesh = m;
        }
        catch
        {
            Mesh();
            Mesh m = new Mesh();
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            MeshCollider collider = gameObject.GetComponent<MeshCollider>();
            m.Clear();
            m.SetVertices(data.points);
            m.SetColors(data.colors);
            m.SetNormals(data.normals);
            m.SetTriangles(data.triangles, 0);

            filter.mesh = m;
            collider.sharedMesh = m;
        }


        if (data.WaterPoints.Count > 0)
        {
            GameObject water = new GameObject();
            water.name = "Chunk Water";
            water.transform.position = gameObject.transform.position;
            //water.transform.SetParent(gameObject.transform);

            Mesh m = new Mesh();

            MeshFilter filter = water.AddComponent<MeshFilter>();
            MeshCollider collider = water.AddComponent<MeshCollider>();
            MeshRenderer renderer = water.AddComponent<MeshRenderer>();

            m.Clear();
            m.SetVertices(data.WaterPoints);
            m.SetColors(data.WaterColors);
            m.SetNormals(data.WaterNormals);
            m.SetTriangles(data.WaterTriangles, 0);

            //probe = water.AddComponent<ReflectionProbe>();
            //probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
            //probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
            //probe.boxProjection = true;
            //probe.center = new Vector3(Constants.ChunkWidth / 2.0f, data.WaterPoints[0].y + 15.0f, Constants.ChunkWidth / 2.0f);
            //probe.size = new Vector3(Constants.ChunkWidth * 4.0f, 30.0f, Constants.ChunkWidth * 4.0f);
            //probe.blendDistance = 10.0f;
            //probe.resolution = 256;
            //probe.RenderProbe();

            renderer.material = Resources.Load<Material>("Water");

            filter.mesh = m;
            collider.sharedMesh = m;
        }
    }

    //float timer = 0.0f;
    //float MaxTime = 1.0f;
    //public void Update()
    //{
    //    if(probe != null)
    //    {
    //        probe.RenderProbe();
    //    }
    //}

    public void Destroy()
    {

    }
}