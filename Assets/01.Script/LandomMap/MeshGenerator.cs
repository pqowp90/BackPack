using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
  public static void GenerateTerrainMesh(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2;
        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                meshData.Vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, y], topLeftZ -y);

                if(x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangles(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangles(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
    }
}
public class MeshData
{
    public Vector3[] Vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int traingleIndex;
    public MeshData(int meshWidth, int meshHeight)
    {
        Vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshHeight * meshWidth];  
        triangles = new int[(meshWidth-1)*(meshHeight-1) * 6];  
    }
    public void AddTriangles(int a,int b,int c)
    {
        triangles[traingleIndex] = a;
        triangles[traingleIndex+1] = b;
        triangles[traingleIndex+2] = c;
        traingleIndex += 3;
    }
}