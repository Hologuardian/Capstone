using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChunkDecorator : MonoBehaviour
{
    public abstract void decorateChunkData(ChunkData data, FastNoise noise);
}