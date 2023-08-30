using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDist = 300;
    public Transform viewer;

    public static Vector2 viewerPosition;
    int chunkSize;
    int chunkVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainDicitionary = new Dictionary<Vector2, TerrainChunk>(); 
    private void Start()
    { 
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVisibleInViewDst =Mathf.RoundToInt(maxViewDist / chunkSize);
    }
    void UpdateVisibleChunks()
    {
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for(int yOffset = -chunkVisibleInViewDst;  yOffset <= chunkVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunkVisibleInViewDst; xOffset <= chunkVisibleInViewDst; xOffset++)
            {
                Vector2 viewChunkCoord = new Vector2(currentChunkCoordX+xOffset,currentChunkCoordY+yOffset);
                if(terrainDicitionary.ContainsKey(viewChunkCoord)) {
                    //
                }
                else {
                    terrainDicitionary.Add(viewChunkCoord, new TerrainChunk());
                }
            }
        }
    }
    public class TerrainChunk
    {

    }
}
