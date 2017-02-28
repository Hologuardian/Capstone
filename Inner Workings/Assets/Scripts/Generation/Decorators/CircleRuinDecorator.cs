using UnityEngine;

public class CircleRuinDecorator : ChunkDecorator
{
    public ChunkManager manager;

    void Awake()
    {
        manager = FindObjectOfType<ChunkManager>();
    }

    const float noiseScale = 8.125f;

    public override void decorateChunkData(ChunkData data, FastNoise noise)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < Constants.ChunkWidth + 1; i++)
        {
            for (int j = 0; j < Constants.ChunkWidth + 1; j++)
            {
                bool tree = false;
                float noiseVal = (noise.GetWhiteNoiseInt(i + data.ChunkX * (Constants.ChunkWidth), j + data.ChunkZ * (Constants.ChunkWidth)) * 0.5f + 0.5f);
                bool shouldTree = noiseVal < 0.000425f;
                uint previous = 0;
                int height = 0;
                for (int k = 0; k < Constants.ChunkHeight; k++)
                {
                    if (!tree && shouldTree && k < 20)
                    {
                        uint c = data.values[(i * (Constants.ChunkWidth + 1) * (Constants.ChunkHeight + 1) + j * (Constants.ChunkHeight + 1) + k)];
                        if (previous > 0 && c % 255 + (c >> 8) % 255 + (c >> 16) % 255 <= 0)
                        {
                            tree = true;
                        }
                        previous = c;
                    }

                    if (tree)
                    {
                        if(height == 2)
                        {
                            //data.values[(i * (Constants.ChunkWidth + 1) * (Constants.ChunkHeight + 1) + j * (Constants.ChunkHeight + 1) + k)] = 0xFF00FFFF;
                            data.interactables.Add(new RuinMemory(new Vector3(i + data.ChunkX * Constants.ChunkWidth, k, j + data.ChunkZ * Constants.ChunkWidth)));
                        }
                        if(height >= 6 && height <= 7)
                        {
                            float radius = 12.0f;
                            float width = 1.5f ;
                            for(float theta = 0; theta < Mathf.PI * 2.0f; theta += Mathf.PI / 128.0f)
                            {
                                float sinX = Mathf.Sin(theta);
                                float cosZ = Mathf.Cos(theta);

                                for (float w = 0.0f; w <= width; w += 0.5f)
                                {
                                    int x = (int)(i + sinX * radius + sinX * w) + data.ChunkX * Constants.ChunkWidth;
                                      int z = (int)(j + cosZ * radius + cosZ * w) + data.ChunkZ * Constants.ChunkWidth;
                                    if ((int)(theta * 750) % 350 < 225)
                                    {
                                        if (noise.GetWhiteNoiseInt(x, k, z) * 500 + 500 > 250)
                                        {
                                            manager.SetBlock(x, k, z, 0x444444FF);
                                        }
                                    }
                                    else if(w < width && w > 0)
                                    {
                                        for (int h = 0; h < k; h++)
                                        {
                                            if (noise.GetWhiteNoiseInt(x , h, z) * 500 + 500 > 150)
                                            {
                                                manager.SetBlock(x, h, z, 0x444444FF);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        height++;
                    }
                }
            }
        }
    }
}
