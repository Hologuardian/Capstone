using UnityEngine;

public class TestTreeDecorator : ChunkDecorator
{
    public ChunkManager manager;

    void Awake()
    {
        manager = FindObjectOfType<ChunkManager>();
    }

    const float noiseScale = 8.125f;
    const int treeWidth = 2;

    public override void decorateChunkData(ChunkData data, FastNoise noise)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < Constants.ChunkWidth + 1; i++)
        {
            for (int j = 0; j < Constants.ChunkWidth + 1; j++)
            {
                bool tree = false;
                float noiseVal = (noise.GetWhiteNoise((i + data.ChunkX * (Constants.ChunkWidth)) / treeWidth, (j + data.ChunkZ * (Constants.ChunkWidth)) / treeWidth) * 0.125f + 0.125f);
                int treeHeight = 7 + (int)((noiseVal) * 2400.0f);
                bool shouldTree = noiseVal < 0.00425f;
                uint previous = 0;
                int height = 0;
                for (int k = 0; k < Constants.ChunkHeight; k++)
                {
                    if (!tree && shouldTree && k < 35 && k > 23)
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
                        if (height < treeHeight)
                        {
                            uint color = 0x8B4513FF - 0x04020100 * (uint)(noise.GetWhiteNoiseInt((i + data.ChunkX * (Constants.ChunkWidth)) * treeWidth, k, (j + data.ChunkZ * (Constants.ChunkWidth)) * treeWidth) * 13.0f + 2.0f);
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
                                        if (Vector3.Distance(center, new Vector3(n, (b - k) * 1.25f + k, m)) < (2.0f + noiseVal * 1500.0f) + rand.NextDouble() * (noiseVal * 150.0f + 0.5f))
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
