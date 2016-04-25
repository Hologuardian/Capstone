using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct ChunkConstuctor
{
    public TerrainData data;
    public Vector4 paramSet;
    public int heightMapRes;
    public int alphaMapRes;
    public ChunkConstuctor(TerrainData d, Vector4 p)
    {
        data = d;
        paramSet = p;
        heightMapRes = d.heightmapResolution;
        alphaMapRes = d.alphamapResolution;
    }
}

