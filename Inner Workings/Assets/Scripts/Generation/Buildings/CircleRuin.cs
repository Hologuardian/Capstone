using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CircleRuin
{
    protected float radius = 12.0f;
    protected float width = 2.0f;

    public float ruinRadius
    {
        get { return radius + width; }
    }

    public void Generate(ChunkData data, ChunkManager manager, FastNoise noise, int i, int j, int k)
    {
        for (float theta = 0; theta < Mathf.PI * 2.0f; theta += Mathf.PI / 128.0f)
        {
            float sinX = Mathf.Sin(theta);
            float cosZ = Mathf.Cos(theta);

            for (float w = 0.0f; w < width; w += 0.5f)
            {
                int x = (int)(i + sinX * radius + sinX * w) + data.ChunkX * Constants.ChunkWidth;
                int z = (int)(j + cosZ * radius + cosZ * w) + data.ChunkZ * Constants.ChunkWidth;

                Build(x, z, w, k, theta, noise, manager);
            }
        }
    }

    public abstract void Build(int x, int z, float w, int k, float theta, FastNoise noise, ChunkManager manager);
}
