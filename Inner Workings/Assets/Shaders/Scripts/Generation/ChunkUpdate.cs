using UnityEngine;
using System.Collections.Generic;

public class ChunkUpdate
{
    public List<long[]> positions;
    public List<uint> values;
    public ChunkUpdate()
    {
        positions = new List<long[]>();
        values = new List<uint>();
    }
}
