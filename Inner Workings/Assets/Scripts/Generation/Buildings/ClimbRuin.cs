using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ClimbRuin : CircleRuin
{
    public override void Build(int x, int z, float w, int k, float theta, FastNoise noise, ChunkManager manager)
    {
        if ((int)(theta * 750) % 350 < 225)
        {
            if (noise.GetWhiteNoiseInt(x, k, z) * 500 + 500 > 250)
            {
                manager.SetBlock(x, k, z, 0x444444FF);
            }
        }
        else if (w < width && w > 0)
        {
            for (int h = 0; h < k; h++)
            {
                if (noise.GetWhiteNoiseInt(x, h, z) * 500 + 500 > 150)
                {
                    manager.SetBlock(x, h, z, 0x444444FF);
                }
            }
        }
    }
}
