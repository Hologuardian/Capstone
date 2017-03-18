using UnityEngine;

public struct ChunkParameters
{
    public int rainFallMax;
    public int rainFallMin;
    public int heightAdjust;
    public bool river;
    public bool lake;

    ChunkParameters(Color32 chunkData, bool river, bool lake)
    {
        rainFallMax = chunkData.b;
        rainFallMin = chunkData.g;
        heightAdjust = chunkData.r;
        this.river = river;
        this.lake = lake;
    }
}