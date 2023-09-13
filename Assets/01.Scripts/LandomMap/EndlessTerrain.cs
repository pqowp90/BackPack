using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDist = 350;
    public Transform viewer;

    public static Vector2 viewerPosition;
    int chunkSize;
    int chunkVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainDicitionary = new Dictionary<Vector2, TerrainChunk>(); 
    List<TerrainChunk> terrainChunksVisbleLastUpdate = new List<TerrainChunk>();
    private void Start()
    { 
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVisibleInViewDst =Mathf.RoundToInt(maxViewDist / chunkSize);
    }
    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);  
        UpdateVisibleChunks();
    }
    void UpdateVisibleChunks()
    {
        for(int i=0;i<terrainChunksVisbleLastUpdate.Count;i++)
        {
            terrainChunksVisbleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisbleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for(int yOffset = -chunkVisibleInViewDst;  yOffset <= chunkVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunkVisibleInViewDst; xOffset <= chunkVisibleInViewDst; xOffset++)
            {
                Vector2 viewChunkCoord = new Vector2(currentChunkCoordX+xOffset,currentChunkCoordY+yOffset);
                if(terrainDicitionary.ContainsKey(viewChunkCoord)) {
                    terrainDicitionary[viewChunkCoord].UpdateTerrainChunk();
                    if (terrainDicitionary[viewChunkCoord].IsVisible())
                    {
                        terrainChunksVisbleLastUpdate.Add(terrainDicitionary[viewChunkCoord]);
                    }
                }
                else {
                    terrainDicitionary.Add(viewChunkCoord, new TerrainChunk(viewChunkCoord,chunkSize,transform));
                }
            }
        }
    }
    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;
        public TerrainChunk(Vector2 coord,int size,Transform parent)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x,0,position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size /10f;
            meshObject.transform.parent= parent; 
            SetVisible(false);
        }
        public void UpdateTerrainChunk()
        {
            float viewrDstFromNearestEnge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewrDstFromNearestEnge <= maxViewDist;
            SetVisible(visible);
        }
        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }
        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }
}