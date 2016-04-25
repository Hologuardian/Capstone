using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public struct TerrainBuilder
{
    public TerrainData data;
    public Vector3 pos;

    public TerrainBuilder(TerrainData d, Vector3 v)
    {
        data = d;
        pos = v;
    }
}
