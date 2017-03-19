using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TowerRuin : CircleRuin
{

    public TowerRuin()
    {
        radius = 3;
        width = 5;
    } 

    public override void Build(int x, int z, float w, int k, float theta, FastNoise noise, ChunkManager manager)
    {
        if ((int)(theta * 550) % 550 < 275)
        {
            ;
        }
        else
        {
            if (w < width && w > 4.0f)
            {
                for (int h = 0; h < k; h++)
                {
                    if (noise.GetWhiteNoiseInt(x, h, z) * 500 + 500 > 250)
                    {
                        manager.SetBlock(x, h, z, 0x444444FF);
                    }
                }
            }
            if (w <= 4.0f && w >= 0.5f)
            {
                for (int h = k - 1; h < k + 1; h++)
                {
                    if (noise.GetWhiteNoiseInt(x, h, z) * 500 + 500 > 250)
                    {
                        manager.SetBlock(x, h, z, 0x444444FF);
                    }
                }
            }
            if (w <= 1.0f)
            {
                for (int h = k; h < k + 15; h++)
                {
                    if (noise.GetWhiteNoiseInt(x, h, z) * 500 + 500 > 250)
                    {
                        manager.SetBlock(x, h, z, 0x444444FF);
                    }
                }
            }
        }
    }
}
