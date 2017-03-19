using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PillarRuin : CircleRuin
{
    public override void Build(int x, int z, float w, int k, float theta, FastNoise noise, ChunkManager manager)
    {
        if ((int)(theta * 550) % 550 < 475)
        {
            ;
        }
        else if (w < width && w > 0)
        {
            for (int h = 0; h < k + 10; h++)
            {
                if (noise.GetWhiteNoiseInt(x, h, z) * 500 + 500 > 250)
                {
                    manager.SetBlock(x, h, z, 0x444444FF);
                }
            }
        }
    }
}
