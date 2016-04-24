using UnityEngine;
using System.Collections;
using System;

public class GenerateHeightmap : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        MakeTerrain(0, 0, 5, 5);
        MakeTerrain(2048, 0, 6, 5);
        MakeTerrain(0, 2048, 5, 6);
        MakeTerrain(2048, 2048, 6, 6);
        MakeTerrain(-2048, 0, 4, 5);
        MakeTerrain(0, -2048, 5, 4);
        MakeTerrain(-2048, -2048, 4, 4);
        MakeTerrain(-2048, 2048, 4, 6);
        MakeTerrain(2048, -2048, 6, 4);

	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void MakeTerrain(float x, float z, int u, int v)
    {
        GameObject obj = new GameObject();
        obj.transform.parent = gameObject.transform;

        float seedx = u;// +Constants.MapSeed;
        float seedz = v;// +Constants.MapSeed;


        TerrainData td = new TerrainData();

        td.size = new Vector3(64, 600, 64);
        td.heightmapResolution = 1024;
        //td.baseMapResolution = 1024;

        //td.SetDetailResolution(1024, 16);

        int heightmapWidth = td.heightmapWidth;
        int heightmapHeight = td.heightmapHeight;
        Debug.Log(String.Format("{0} :: {1} :: {2} :: {3} :: {4} :: {5}", seedx, seedz, td.heightmapWidth, td.heightmapHeight, x, z));

        float[,] heightMap = new float[heightmapWidth, heightmapHeight];

        //float hi = 0.00f;
        string debug = "";
        string debug2 = "";
        string debug3 = "";
        string debug4 = "";
        for (int i = 0; i < heightmapWidth; i++)
        {
            //string s = "";
            for (int j = 0; j < heightmapHeight; j++)
            {
                float h = 0;//hi += (Random.value - 0.5f) / 100.0f);
                //float xCoord = seedx + i;// +i / (td.heightmapResolution - 1);
                //float zCoord = seedz + j;// +j / (td.heightmapResolution - 1);
                var xCoord = (float)((td.size.x * seedx) + i * td.size.x / heightmapWidth) / (heightmapWidth - 1);
                var zCoord = (float)((td.size.z * seedz) + j * td.size.z / heightmapHeight) / (heightmapHeight - 1);
                h = Mathf.PerlinNoise(xCoord, zCoord) * 0.5f;
                h += Mathf.PerlinNoise(xCoord * 2, zCoord * 2) * 0.1f;
                h += Mathf.Max(0.0f, Mathf.PerlinNoise(xCoord * 0.8f, zCoord * 0.8f) - 0.5f);
                Mathf.Min(h, 1.0f);
                heightMap[j, i] = h;
                    
                //if (i % Random.Range(8, 16) == 0 && j % Random.Range(8, 16) == 0)
                //    Instantiate(smallTree, new Vector3(i * 4.0f, h * 600.0f + 8.0f, j * 4.0f), Quaternion.identity);
                //s += h + " ";
                if(i == 0)
                {
                    debug +=  "::" + h + " " + xCoord;
                }
                if (i == 1)
                {
                    debug2 += "::" + h + " " + xCoord;
                }
                if (i == heightmapWidth - 2)
                {
                    debug3 += "::" + h + " " + xCoord;
                }
                if (i == heightmapWidth - 1)
                {
                    debug4 += "::" + h + " " + xCoord;
                }
            }
        }
        Debug.Log(debug);
        Debug.Log(debug2);
        Debug.Log(debug3);
        Debug.Log(debug4);

        td.SetHeights(0, 0, heightMap);

        var tObj = Terrain.CreateTerrainGameObject(td);
        tObj.transform.parent = obj.transform;
        Terrain t = tObj.GetComponent<Terrain>();
        t.heightmapPixelError = 8;
        t.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        t.Flush();
        obj.transform.position = new Vector3(x, 0, z);
    }

    public Vector2 WorldToHeightMap(Vector3 pos)
    {
        return new Vector2(pos.x, pos.z);
    }
}
