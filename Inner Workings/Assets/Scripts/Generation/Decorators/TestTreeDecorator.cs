using UnityEngine;

public class TestTreeDecorator : ChunkDecorator
{
    public ChunkManager manager;

    void Awake()
    {
        manager = FindObjectOfType<ChunkManager>();
    }

    public override void decorateChunkData(ChunkData data, FastNoise noise)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < Constants.ChunkWidth + 1; i++)
        {
            for (int j = 0; j < Constants.ChunkWidth + 1; j++)
            {
                bool tree = false;
                float noiseVal = (noise.GetWhiteNoise(i + data.ChunkX * (Constants.ChunkWidth), j + data.ChunkZ * (Constants.ChunkWidth)) * 0.5f + 0.5f);
                bool shouldTree = noiseVal < 0.00425f;
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
                        height++;
                        int treeHeight = 14 + (int)(noise.GetWhiteNoise(i, j) * 7.0f);
                        float noiseScale = 8.125f;
                        if (height < treeHeight)
                        {
                            uint color = 0x8B4513FF;
                            data.values[(i * (Constants.ChunkWidth + 1) * (Constants.ChunkHeight + 1) + j * (Constants.ChunkHeight + 1) + k)] = color;
                        }
                        else if(height == treeHeight)
                        {
                            Vector3 center = new Vector3(i, k, j);
                            int width = 10;
                            for(int n = i - width; n <= i + width; n++)
                            {
                                for (int m = j - width; m <= j + width; m++)
                                {
                                    for(int b = k - width; b <= k + width; b++)
                                    {
                                        if (Vector3.Distance(center, new Vector3(n, b, m)) < (noise.GetPerlin(n * noiseScale, b * noiseScale, m * noiseScale) * 1.75f + 1.75f) + (4.0f + noise.GetWhiteNoise(i, j) * 2.0f) + rand.NextDouble() * (noise.GetWhiteNoise(i, j) * 0.25f + 0.5f))
                                        {
                                            manager.SetBlock(n + data.ChunkX * Constants.ChunkWidth, b, m + data.ChunkZ * Constants.ChunkWidth, 0x3B5323FF);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
