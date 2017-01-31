using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ChunkData
{
    public List<Vector3> points;
    public List<uint> values;
    public List<int> triangles;
    public List<Vector3> normals;
    public List<Color32> colors;
    public int ChunkX;
    public int ChunkZ;
    public ChunkData()
    {
        points = new List<Vector3>();
        values = new List<uint>();
        triangles = new List<int>();
        normals = new List<Vector3>();
        colors = new List<Color32>();
    }
}