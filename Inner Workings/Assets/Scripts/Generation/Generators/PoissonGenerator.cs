using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine;

public delegate float PoissonDistribution(float x, float z, float v);
public class PoissonGenerator
{
    public static List<Vector3> GeneratePoisson(int width, int height, float minSize, float maxSize, PoissonDistribution dist, int numPoints)
    {
        UnityEngine.Random.InitState(Constants.seed);
        int cellSize = Mathf.Max(1, (int)(minSize / Mathf.Sqrt(2.0f)));
        float size = UnityEngine.Random.value;
        float x = UnityEngine.Random.Range(width * 0.1f, width * 0.9f);
        float z = UnityEngine.Random.Range(height * 0.1f, height * 0.9f);

        size = dist(x, z, size);

        size *= maxSize - minSize;
        size += minSize;

        Vector3[] grid = new Vector3[Mathf.CeilToInt(width / cellSize) * Mathf.CeilToInt(height / cellSize)];

        for(int i = 0; i < grid.Length; i++)
        {
            grid[i] = Vector3.zero;
        }

        
        List<Vector2> processList = new List<Vector2>();
        List<Vector3> samplePoints = new List<Vector3>();

        Vector3 firstPoint = new Vector3(x, z, size);

        processList.Add(firstPoint);
        samplePoints.Add(firstPoint);
        grid[imageToGrid(firstPoint, width, cellSize)] = firstPoint;

        while (processList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, processList.Count);
            Vector2 point = processList[index];
            processList.RemoveAt(index);

            for (int i = 0; i < numPoints; i++)
            {
                size = UnityEngine.Random.value;

                size = dist(point.x, point.y, size);

                size *= maxSize - minSize;
                size += minSize;

                Vector3 newPoint = generateRandomPointAround(point, size);
                //check that the point is in the image region
                //and no points exists in the point's neighbourhood
                if (inRectangle(newPoint, width, cellSize, grid.Length) && !inNeighbourhood(grid, newPoint, width, height, size, cellSize))
                {
                    //update containers
                    processList.Add(newPoint);
                    samplePoints.Add(newPoint);
                    try
                    {
                        grid[imageToGrid(newPoint, width, cellSize)] = newPoint;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.Log("Grid point out of range: " + newPoint.x + ":" + newPoint.y + ":" + newPoint.z);
                        continue;
                    }
                }
            }
        }

        return samplePoints;
    }
    
    private static int imageToGrid(Vector2 point, int w, int cellSize)
    {
        int gridX = (int)(point.x / (float)cellSize);
        int gridY = (int)(point.y / (float)cellSize);
        return gridX + gridY * Mathf.CeilToInt((float)w / (float)cellSize);
    }

    private static bool inRectangle(Vector2 point, int w, int cellSize, int gridSize)
    {
        int index = imageToGrid(point, w, cellSize);
        if (index < gridSize && index > 0)
            return true;
        return false;
    }

    private const int checkRadius = 10;
    private static bool inNeighbourhood(Vector3[] grid, Vector2 point, int w, int h, float size, int cellSize)
    {
        int gridPoint = imageToGrid(point, w, cellSize);

        //get the neighbourhood of the point in the grid
        List<Vector3> cellsAroundPoint = new List<Vector3>();
        for(int i = (gridPoint % (Mathf.CeilToInt((float)w / (float)cellSize))) - checkRadius; i <= (gridPoint % (Mathf.CeilToInt((float)w / (float)cellSize))) + checkRadius; i++)
        {
            for (int j = (int)(gridPoint / (Mathf.CeilToInt((float)w / (float)cellSize))) - checkRadius; j <= (int)(gridPoint / (Mathf.CeilToInt((float)w / (float)cellSize))) + checkRadius; j++)
            {
                if (i < 0 || j < 0)
                    continue;
                if (i >= Mathf.CeilToInt((float)w / (float)cellSize) || j >= Mathf.CeilToInt((float)h / (float)cellSize))
                    continue;
                try
                {
                    cellsAroundPoint.Add(grid[i + j * Mathf.CeilToInt((float)w / (float)cellSize)]);
                }
                catch(IndexOutOfRangeException e)
                {
                    Debug.Log("Grid point out of range: " + i + ":" + j);
                    continue;
                }
            }
        }

        //Check neighbouring cells for if they are too close
        foreach (Vector3 cell in cellsAroundPoint)
        {
            if (Vector2.Distance(new Vector2(cell.x, cell.y), point) < Mathf.Max(size, cell.z))
            {
                return true;
            }
        }

        return false;
    }

    private static Vector3 generateRandomPointAround(Vector2 point, float size)
    {
        float r1 = UnityEngine.Random.value; //random point between 0 and 1
        float r2 = UnityEngine.Random.value;
        //random radius between minSize and 2 * minSize
        float radius = size * (r1 + 1);
        //random angle
        float angle = 2 * Mathf.PI * r2;
        //the new point is generated around the point (x, y)
        float newX = point.x + radius * Mathf.Cos(angle);
        float newY = point.y + radius * Mathf.Sin(angle);
        return new Vector3(newX, newY, size);
    }
}
