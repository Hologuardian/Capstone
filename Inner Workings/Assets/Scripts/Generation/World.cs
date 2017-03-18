using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Threading;

public class World : MonoBehaviour
{
    private FastNoise noise;

    public Dictionary<long, ChunkParameters> worldData;
    public Texture2D data;
    private Color32[] dataColors;
    private Thread[] workers = new Thread[8];
    public ComputeShader shader;
    public RenderTexture computeTex;
    public RenderTexture flowTex;

    public int ready = 0;
    bool done = false;
    bool abort = false;
    long startTime;

    void Start()
    {
        data = new Texture2D(Constants.WorldSize, Constants.WorldSize, TextureFormat.ARGB32, false);
        dataColors = new Color32[Constants.WorldSize * Constants.WorldSize];
        startTime = DateTime.Now.Ticks;
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                int n = i;
                int m = j;
                workers[i + j * 4] = new Thread(() => GenerateData(n, m));
                workers[i + j * 4].Start();
            }
        }
    }

    void OnDisable()
    {
        abort = true;
    }

    void Update()
    {
        if(ready >= 8 && !done)
        {
            Debug.Log("Finished Generation in " + ((DateTime.Now.Ticks - startTime) / 10000000.0f) + " seconds!");
            Debug.Log("Generation ready: beginning erosion... ");
            done = true;

            data.SetPixels32(dataColors);
            data.Apply();
            long time = DateTime.Now.Ticks;
            byte[] bytes = data.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../WorldScaleBefore.png", bytes);

            computeTex = new RenderTexture(Constants.WorldSize, Constants.WorldSize, 0, RenderTextureFormat.ARGB32);
            computeTex.enableRandomWrite = true;
            computeTex.Create();

            flowTex = new RenderTexture(Constants.WorldSize, Constants.WorldSize, 0, RenderTextureFormat.ARGB32);
            flowTex.enableRandomWrite = true;
            flowTex.Create();
            const int threadX = 8;
            const int threadY = 8;
            int kernelHandle;

            kernelHandle = shader.FindKernel("SetUpErosion");

            shader.SetTexture(kernelHandle, "Input", data);
            shader.SetTexture(kernelHandle, "Result", computeTex);
            shader.SetTexture(kernelHandle, "Flow", flowTex);

            shader.Dispatch(kernelHandle, Constants.WorldSize / threadX, Constants.WorldSize / threadY, 1);

            for (int i = 0; i < 96; i++)
            {
                kernelHandle = shader.FindKernel("Rainfall");

                shader.SetTexture(kernelHandle, "Input", data);
                shader.SetTexture(kernelHandle, "Result", computeTex);
                shader.SetTexture(kernelHandle, "Flow", flowTex);

                shader.Dispatch(kernelHandle, Constants.WorldSize / threadX, Constants.WorldSize / threadY, 1);

                kernelHandle = shader.FindKernel("FlowSetup");

                shader.SetTexture(kernelHandle, "Input", data);
                shader.SetTexture(kernelHandle, "Result", computeTex);
                shader.SetTexture(kernelHandle, "Flow", flowTex);

                shader.Dispatch(kernelHandle, Constants.WorldSize / threadX, Constants.WorldSize / threadY, 1);

                kernelHandle = shader.FindKernel("WaterMotion");

                shader.SetTexture(kernelHandle, "Input", data);
                shader.SetTexture(kernelHandle, "Result", computeTex);
                shader.SetTexture(kernelHandle, "Flow", flowTex);

                shader.Dispatch(kernelHandle, Constants.WorldSize / threadX, Constants.WorldSize / threadY, 1);

                kernelHandle = shader.FindKernel("Evaporation");

                shader.SetTexture(kernelHandle, "Input", data);
                shader.SetTexture(kernelHandle, "Result", computeTex);
                shader.SetTexture(kernelHandle, "Flow", flowTex);

                shader.Dispatch(kernelHandle, Constants.WorldSize / threadX, Constants.WorldSize / threadY, 1);
            }

            //for(int i = 0; i < 16; i++)
            //{
            //    kernelHandle = shader.FindKernel("FlowSetup");

            //    shader.SetTexture(kernelHandle, "Input", data);
            //    shader.SetTexture(kernelHandle, "Result", computeTex);
            //    shader.SetTexture(kernelHandle, "Flow", flowTex);

            //    shader.Dispatch(kernelHandle, Constants.WorldSize / threadX, Constants.WorldSize / threadY, 1);

            //    kernelHandle = shader.FindKernel("WaterMotion");

            //    shader.SetTexture(kernelHandle, "Input", data);
            //    shader.SetTexture(kernelHandle, "Result", computeTex);
            //    shader.SetTexture(kernelHandle, "Flow", flowTex);

            //    shader.Dispatch(kernelHandle, Constants.WorldSize / threadX, Constants.WorldSize / threadY, 1);
            //}

            RenderTexture.active = computeTex;
            data.ReadPixels(new Rect(0, 0, Constants.WorldSize, Constants.WorldSize), 0, 0);
            RenderTexture.active = null;

            float seconds = (DateTime.Now.Ticks - time) / 10000000.0f;
            Debug.Log("Erosion finished in " + seconds + " seconds!");

            bytes = data.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../WorldScaleAfter.png", bytes);


            RenderTexture.active = flowTex;
            data.ReadPixels(new Rect(0, 0, Constants.WorldSize, Constants.WorldSize), 0, 0);
            RenderTexture.active = null;

            bytes = data.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../WorldScaleFlow.png", bytes);
        }
    }

    void GenerateData(int n, int m)
    {
        Debug.Log("Starting generation for chunk " + n + ":" + m);
        noise = new FastNoise(Constants.seed);
        int fourthWorldSize = Constants.WorldSize / 4;
        int halfWorldSize = Constants.WorldSize / 2;
        
        for (int i = n * fourthWorldSize; i < (n + 1) * fourthWorldSize; i++)
        {
            if (abort)
                return;
            for (int j = m * halfWorldSize; j < (m + 1) * halfWorldSize; j++)
            {
                float rainMax = Mathf.Clamp01(noise.GetPerlin(i * 2.0f, j * 2.0f, 10) * 0.125f + 0.125f);
                float evaporation = Mathf.Clamp01(noise.GetPerlin(i * 4.0f, j * 4.0f, 200) * 0.025f + 0.05f);
                float h = GetHeight(i, j);
                if (noise.GetWhiteNoiseInt(i, j) > 0.99f && h > 0.35f)
                    rainMax = 1.0f;
                dataColors[i + j * Constants.WorldSize] = new Color32((byte)(h * 255.0f), (byte)(evaporation * 255.0f), (byte)(rainMax * 255.0f), 255);
                //dataColors[i + j * Constants.WorldSize] = new Color32(0, (byte)(rainMin * 255.0f), (byte)(rainMax * 255.0f), 255);
            }
           Debug.Log("Generating world chunk " + (n + m * 4) + "... " + (int)((i - n * fourthWorldSize) / (fourthWorldSize / 100.0f)) + "%");
        }
        Debug.Log("Generated Section! " + n + ":" + m);
        ready++;
    }

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
    const float width = Constants.ChunkWidth * 0.5f;

    float GetHeight(int i, int j)
    {
        float noiseH = (noise.GetPerlinFractal((width * i) * octave1, (width * j) * octave1) * octave1Multiplier + octave1Value);
        noiseH *= (noise.GetPerlinFractal((width * i) * octave2, (width * j) * octave2) * octave2Multiplier + octave2Value);
        float biome = (noise.GetNoise((width * i) * octave3, (width * j) * octave3) * octave3Multiplier + octave3Value);
        biome *= preBiomeEffect;
        biome *= biome;
        biome *= postBiomeEffect;
        biome = Mathf.Sqrt(biome);
        noiseH *= biome;
        noiseH *= (noise.GetPerlinFractal((width * i) * octave4, (width * j) * octave4) * octave4Multiplier + octave4Value);
        noiseH *= (noise.GetPerlinFractal((width * i) * octave5, (width * j) * octave5) * octave5Multiplier + octave5Value);
        noiseH *= (noise.GetPerlinFractal((width * i) * octave6, (width * j) * octave6) * octave5Multiplier + octave5Value);

        return Mathf.Clamp01(noiseH + 0.1f);
    }
}