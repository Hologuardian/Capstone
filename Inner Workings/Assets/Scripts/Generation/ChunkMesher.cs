using System;
using System.Collections.Generic;

public abstract class ChunkMesher
{
	public abstract void MeshChunk(ChunkData data);
    public abstract List<SimpleVertex> MeshArray(uint[] data, int w, int l, int h);
};