using System.Collections.Generic;
using UnityEngine;

public class CircleRuinDecorator : ChunkDecorator
{
    public ChunkManager manager;
    public List<CircleRuin> ruins;

    void Awake()
    {
        manager = FindObjectOfType<ChunkManager>();

        ruins = new List<CircleRuin>();
        ruins.Add(new StonehengeRuin());
        ruins.Add(new PillarRuin());
        ruins.Add(new TowerRuin());
    }

    const float noiseScale = 8.125f;

    public override void decorateChunkData(ChunkData data, FastNoise noise)
    {
        for (int i = 0; i < Constants.ChunkWidth + 1; i++)
        {
            for (int j = 0; j < Constants.ChunkWidth + 1; j++)
            {
                float chance = 0.000425f;
                bool tree = false;
                float noiseVal = (noise.GetWhiteNoiseInt(i + data.ChunkX * (Constants.ChunkWidth), j + data.ChunkZ * (Constants.ChunkWidth)) * 0.5f + 0.5f);
                bool shouldTree = noiseVal < chance;
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
                            data.interactables.Add(new RuinMemory(new Vector3(i, k, j), ruins[(int)((noiseVal / chance) * ruins.Count)].ruinRadius));
                        }
                        if(height == 7)
                        {
                            ruins[(int)((noiseVal / chance) * ruins.Count)].Generate(data, manager, noise, i, j, k);
                        }
                        height++;
                    }
                }
            }
        }
    }
}
