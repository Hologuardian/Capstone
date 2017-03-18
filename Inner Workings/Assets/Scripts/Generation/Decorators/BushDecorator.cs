using UnityEngine;

public class BushDecorator : ChunkDecorator
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
                float noiseVal = (noise.GetWhiteNoise((i + data.ChunkX * (Constants.ChunkWidth)) / treeWidth, (j + data.ChunkZ * (Constants.ChunkWidth)) / treeWidth) * 0.5f + 0.5f);
                int treeHeight = 7 + (int)((noiseVal) * 2400.0f);
                bool shouldTree = noiseVal > 0.975;
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
                        if(height == 0)
                        {
                            Vector3 center = new Vector3(i, k, j);
                            int width = 3;
                            for(int n = i - width; n <= i + width; n++)
                            {
                                for (int m = j - width; m <= j + width; m++)
                                {
                                    for(int b = k; b <= k + width; b++)
                                    {
                                        if (Vector3.Distance(center, new Vector3(n, ((b - k) * 1.25f) + k, m)) < (((noiseVal - 0.975) / 0.025) * 1.5f) + rand.NextDouble() * 0.5f)
                                        {
                                            manager.SetBlock(n + data.ChunkX * Constants.ChunkWidth, b, m + data.ChunkZ * Constants.ChunkWidth, 0x3B5323FF);
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
