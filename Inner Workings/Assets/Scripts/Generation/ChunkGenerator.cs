using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChunkGenerator : MonoBehaviour
{
    public abstract void generateChunkData(ChunkData data, FastNoise noise);
}