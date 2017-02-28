using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTestGenerator : ChunkGenerator
{
    const float octave1 = 1.0f;
    const float octave1Multiplier = 0.25f;
    const float octave1Value = 0.25f;

    public override void generateChunkData(ChunkData data, FastNoise noise)
    {
        FastNoise noise2 = new FastNoise(noise.GetSeed());
        noise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Natural);
        noise.SetCellularReturnType(FastNoise.CellularReturnType.Distance);
        noise2.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Euclidean);
        noise2.SetCellularReturnType(FastNoise.CellularReturnType.Distance2Sub);
        for (int i = 0; i < Constants.ChunkWidth + 1; i++)
        {
            for (int j = 0; j < Constants.ChunkWidth + 1; j++)
            {
                float noiseH = (noise.GetCellular((i + Constants.ChunkWidth * data.ChunkX) * octave1, (j + Constants.ChunkWidth * data.ChunkZ) * octave1) * octave1Multiplier + octave1Value) * (float)Constants.ChunkHeight;

                //noiseH += (noise2.GetCellular((i + Constants.ChunkWidth * data.ChunkX) * octave1, (j + Constants.ChunkWidth * data.ChunkZ) * octave1) * octave1Multiplier + octave1Value) * (float)Constants.ChunkHeight;

                for (int k = 0; k < Constants.ChunkHeight + 1; k++)
                {
                    //if (noiseH >= k - 2 && noiseH <= k)
                    if (noiseH >= k)
                    {
                        uint color = 0;

                        if (k > 20)
                        {
                            if (k >= 50)
                            {
                                color = 0xEEEEFFFF;
                            }
                            else
                            {
                                color += ((uint)(170 - (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 24;
                                color += ((uint)(170 - (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 16;
                                color += ((uint)(170 - (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 8;
                                color += 255;
                            }
                        }
                        //else if (k <= 10)
                        //{
                        //    color += ((uint)(255 - (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 24;
                        //    color += ((uint)(255 - (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 16;
                        //    color += ((uint)(225 - (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 8;
                        //    color += 255;
                        //}
                        else
                        {
                            color += 0 << 24;
                            color += ((uint)(150 - (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 16;
                            color += 0 << 8;
                            color += 255;
                        }

                        //float noiseC = noise.GetGradient((i + Constants.ChunkWidth * n) * 10.0f, (k)* 10.0f, (j + Constants.ChunkWidth * m) * 10.0f);
                        data.values.Add(color);
                    }
                    //else if (k <= (Constants.ChunkHeight + 1) / 32)
                    //{
                    //    uint color = 0;
                    //    color += 0 << 24;
                    //    color += 0 << 16;
                    //    color += ((uint)(160 + (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 8;
                    //    color += 255;
                    //    data.values.Add(color);
                    //}
                    else if (k <= 0)
                    {
                        uint color = 0;
                        color += 0 << 24;
                        color += ((uint)(150 - (noiseH / (float)Constants.ChunkHeight) * 255.0f) % 255) << 16;
                        color += 0 << 8;
                        color += 255;
                        data.values.Add(color);
                    }
                    else
                        data.values.Add(0);
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
