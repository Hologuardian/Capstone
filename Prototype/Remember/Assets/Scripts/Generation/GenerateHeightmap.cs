using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenerateHeightmap : MonoBehaviour {
    private static System.Random rand = new System.Random((int)(DateTime.UtcNow.Ticks % int.MaxValue));
    private List<Vector2> genOrder = new List<Vector2>();

    private float seed;
    private int numChunks = 0;
    private int lastSeenChunks = 0;
	// Use this for initialization
	void Start () 
    {
        seed = (float)rand.Next(99999);
        StartCoroutine("MakeTerrain", new Vector4(0,0,0,0));
        int max = 32;
        for (int n = 1; n < max; n++)
        {
            for(int i = 0; i <= n; i++)
            {
                genOrder.Add(new Vector2(i, n));
            }
            for (int i = 0; i <= n; i++)
            {
                genOrder.Add(new Vector2(n, i));
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(numChunks > lastSeenChunks && genOrder.Count > 0)
        {
            Vector2 v = genOrder[0];
            StartCoroutine("MakeTerrain", new Vector4(v.x * 512, v.y * 512, 128 * v.x, 128 * v.y));
            genOrder.RemoveAt(0);
            lastSeenChunks = numChunks;
        }
	}

    public Texture2D grass = null;
    public Texture2D rock = null;
    IEnumerator MakeTerrain(Vector4 d)
    {
        float x, z, u, v;
        x = d.x;
        z = d.y;
        u = d.z;
        v = d.w;
        GameObject obj = new GameObject();
        obj.transform.parent = gameObject.transform;

        float seedx = u + seed;// +Constants.MapSeed;
        float seedz = v + seed;// +Constants.MapSeed;


        TerrainData td = new TerrainData();

        td.size = new Vector3(128, 600, 128);
        td.heightmapResolution = 129;
        td.alphamapResolution = 129;
        //td.baseMapResolution = 1024;

        //td.SetDetailResolution(1024, 16);

        var flatSplat = new SplatPrototype();
        var steepSplat = new SplatPrototype();
        flatSplat.texture = grass;
        steepSplat.texture = rock;

        td.splatPrototypes = new SplatPrototype[]
        {
        flatSplat,
        steepSplat
        };
 

        int heightmapWidth = td.heightmapWidth;
        int heightmapHeight = td.heightmapHeight;
        int alphaWidth = td.alphamapWidth;
        int alphaHeight = td.alphamapHeight;

        float[,] heightMap = new float[heightmapWidth, heightmapHeight];
        yield return new WaitForSeconds(0);

        for (int i = 0; i < heightmapWidth; i++)
        {
            for (int j = 0; j < heightmapHeight; j++)
            {
                var xCoord = seedx + i;
                var zCoord = seedz + j;
                float h = Mathf.PerlinNoise(xCoord / 1000.0f, zCoord / 1000.0f) * 0.1f;//hi += (Random.value - 0.5f) / 100.0f);
                h += Mathf.PerlinNoise(xCoord / 600.0f, zCoord / 600.0f) * 0.1f;//hi += (Random.value - 0.5f) / 100.0f);
                h += Mathf.PerlinNoise(xCoord / 150.0f, zCoord / 150.0f) * 0.8f;//hi += (Random.value - 0.5f) / 100.0f);
                h += (Mathf.PerlinNoise(xCoord / 40.0f, zCoord / 40.0f) * 0.15f) * h;//hi += (Random.value - 0.5f) / 100.0f);
                h += (Mathf.PerlinNoise(xCoord / 15.0f, zCoord / 15.0f) * 0.05f) * Mathf.Clamp01((h - 0.5f) * 2.0f);//hi += (Random.value - 0.5f) / 100.0f);
                //h *= 0.5f;
                h = Mathf.Clamp01(h);
                float s = Mathf.PerlinNoise(xCoord / 1000.0f, zCoord / 1000.0f) * 0.95f + 0.05f;
                h = h * (s * s * s * (s * (s * 6 - 15) + 10));
                heightMap[j, i] = h;
                SpawnDetails((int)(u + i) * 4, (int)(v + j) * 4, (int)(h * 600));
            }
        }
        yield return new WaitForSeconds(0);

        td.SetHeights(0, 0, heightMap);

        td.RefreshPrototypes();

        float[,,] splatMap = new float[td.alphamapResolution, td.alphamapResolution, 2];

        for (int i = 0; i < alphaWidth; i++)
        {
            for (int j = 0; j < alphaHeight; j++)
            {
                float xCoord = (float)i / (float)(alphaWidth - 1);
                float zCoord = (float)j / (float)(alphaHeight - 1);

                float steepness = td.GetSteepness(xCoord, zCoord) / 90.0f;


                splatMap[j, i, 0] = 0.8f - steepness;
                splatMap[j, i, 1] = Mathf.Clamp(steepness * 1.25f, 0.25f, 1.0f);
            }
        }
        yield return new WaitForSeconds(0);
        td.SetAlphamaps(0, 0, splatMap);


        var tObj = Terrain.CreateTerrainGameObject(td);
        tObj.transform.parent = obj.transform;
        Terrain t = tObj.GetComponent<Terrain>();
        t.name = "Terrain" + x / td.size.x + "" + z / td.size.z;
        t.heightmapPixelError = 8;
        t.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        t.Flush();

        obj.transform.position = new Vector3(x, 0, z);
        numChunks++;
        yield return new WaitForSeconds(0);
    }

    public Vector2 WorldToHeightMap(Vector3 pos)
    {
        return new Vector2(pos.x, pos.z);
    }


    public GameObject tree = null;
    void SpawnDetails(int x, int z, int h)
    {
        if(h < 250 && x % (rand.Next(10) + 20) == 0 && z % (rand.Next(10) + 20) == 0)
        {
            Instantiate(tree, new Vector3(x, h + 5, z), Quaternion.identity);
        }
    }
}