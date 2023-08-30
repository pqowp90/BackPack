using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap , colorMap, Mesh };
    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultipier;
    public AnimationCurve meshHeightCurve;   

    public bool autoUpdate;

    public TerrainType[] regions; //Áö¿ªµé
    public void GenerateMap()
    {   
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for(int y=0; y<mapChunkSize; y++) { 
            for(int x=0; x<mapChunkSize; x++) { 
                float currentHeight = noiseMap[x,y];
                for(int i  = 0; i < regions.Length; i++){
                    if(currentHeight <= regions[i].height){
                        colorMap[y* mapChunkSize+ x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay disPlay = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            disPlay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.colorMap)
            disPlay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.Mesh)
            disPlay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultipier, meshHeightCurve,levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));


    }
    void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;

        if (octaves < 0)
            octaves = 0;
    }
}
[Serializable]
public struct TerrainType{
    public string name;     
    public float height;
    public Color color;
}