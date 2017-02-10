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
                bool shouldTree = noiseVal < 0.025f;
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
                        if (height < 10)
                        {
                            uint color = 0x8B4513FF;
                            data.values[(i * (Constants.ChunkWidth + 1) * (Constants.ChunkHeight + 1) + j * (Constants.ChunkHeight + 1) + k)] = color;
                        }
                        else if(height == 10)
                        {
                            Vector3 center = new Vector3(i, k, j);
                            for(int n = i - 4; n <= i + 4; n++)
                            {
                                for (int m = j - 4; m <= j + 4; m++)
                                {
                                    for(int b = k - 4; b <= k + 4; b++)
                                    {
                                        if (Vector3.Distance(center, new Vector3(n, b, m)) < rand.NextDouble() * 0.75f + 3.0f)
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
