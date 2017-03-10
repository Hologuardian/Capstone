using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGenerator : ChunkGenerator
{
    const float octave1 = 0.5f;
    const float octave1Multiplier = 0.75f;
    const float octave1Value = 0.35f;

    const float octave2 = 0.25f;
    const float octave2Multiplier = 0.25f;
    const float octave2Value = 0.5f;

    const float octave4 = 2.0f;
    const float octave4Multiplier = 0.5f;
    const float octave4Value = .75f;

    const float octave5 = 1.0f;
    const float octave5Multiplier = 0.25f;
    const float octave5Value = 1.0f;

    const float octave6 = 4.0f;
    const float octave6Multiplier = 0.125f;
    const float octave6Value = 0.5f;

    const float octave3 = 0.0625f; //Reserved Biome Octave
    const float octave3Value = 0.5f;
    const float octave3Multiplier = 0.5f;

    const float preBiomeEffect = 2.25f;
    const float postBiomeEffect = 0.33333333333f;

    public override void generateChunkData(ChunkData data, FastNoise noise)
    {
        for (int i = 0; i < Constants.ChunkWidth + 1; i++)
        {
            for (int j = 0; j < Constants.ChunkWidth + 1; j++)
            {
                float noiseH = (noise.GetPerlinFractal((i + Constants.ChunkWidth * data.ChunkX) * octave1, (j + Constants.ChunkWidth * data.ChunkZ) * octave1) * octave1Multiplier + octave1Value) * (float)Constants.ChunkHeight;
                noiseH *= (noise.GetPerlinFractal((i + Constants.ChunkWidth * data.ChunkX) * octave2, (j + Constants.ChunkWidth * data.ChunkZ) * octave2) * octave2Multiplier + octave2Value);
                float biome = (noise.GetNoise((i + Constants.ChunkWidth * data.ChunkX) * octave3, (j + Constants.ChunkWidth * data.ChunkZ) * octave3) * octave3Multiplier + octave3Value);
                biome *= preBiomeEffect;
                biome *= biome;
                biome *= postBiomeEffect;
                biome = Mathf.Sqrt(biome);
                noiseH *= biome;
                noiseH *= (noise.GetPerlinFractal((i + Constants.ChunkWidth * data.ChunkX) * octave4, (j + Constants.ChunkWidth * data.ChunkZ) * octave4) * octave4Multiplier + octave4Value);
                noiseH *= (noise.GetPerlinFractal((i + Constants.ChunkWidth * data.ChunkX) * octave5, (j + Constants.ChunkWidth * data.ChunkZ) * octave5) * octave5Multiplier + octave5Value);
                noiseH *= (noise.GetPerlinFractal((i + Constants.ChunkWidth * data.ChunkX) * octave6, (j + Constants.ChunkWidth * data.ChunkZ) * octave6) * octave5Multiplier + octave5Value);
                for (int k = 0; k < Constants.ChunkHeight + 1; k++)
                {
                    //if (noiseH >= k - 2 && noiseH <= k)
                    if (noiseH >= k)
                    {
                        uint color = 0;

                        if (k > 20)
                        {
                            if (k >= 60)
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