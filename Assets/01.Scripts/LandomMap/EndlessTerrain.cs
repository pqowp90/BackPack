    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewMoveThresholdForUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
    public LODInfo[] detailLevels;
    public static float maxViewDist;
    public Transform viewer;
    public Material mapMaterial;
    public static Vector2 viewerPosition;
    Vector2 viewPositionOld;
    static MapGenerator mapGenerator;
    float chunkSize;
    int chunkVisibleInViewDst;
    Dictionary<Vector2, TerrainChunk> terrainDicitionary = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunksVisbleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        maxViewDist = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        chunkSize = (MapGenerator.mapChunkSize - 1);
        chunkVisibleInViewDst = Mathf.RoundToInt(maxViewDist / chunkSize);
        UpdateVisibleChunks();
    }
    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        if((viewPositionOld - viewerPosition).sqrMagnitude > sqrViewMoveThresholdForUpdate)
        {
            viewPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }
    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksVisbleLastUpdate.Count; i++)
        {
            terrainChunksVisbleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisbleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunkVisibleInViewDst; yOffset <= chunkVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunkVisibleInViewDst; xOffset <= chunkVisibleInViewDst; xOffset++)
            {
                Vector2 viewChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                if (terrainDicitionary.ContainsKey(viewChunkCoord))
                {
                    terrainDicitionary[viewChunkCoord].UpdateTerrainChunk();
                }
                else
                {
                    terrainDicitionary.Add(viewChunkCoord, new TerrainChunk(viewChunkCoord, chunkSize,detailLevels, transform,mapMaterial));
                }
            }
        }
    }
    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;

        MapData mapData;
        bool mapDataReceived;
        int previousLODIndex = -1;

        public TerrainChunk(Vector2 coord, float size, LODInfo[] detailLevels, Transform parent, Material material)
        {
            this.detailLevels = detailLevels;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            
            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;

            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod,UpdateTerrainChunk);
            }
            mapGenerator.RequestMapData(position,OnMapDataReceived);
        }
        //void OnMeshDataReceived(MeshData meshData)
        //{
        //    meshFilter.mesh = meshData.CreatMesh();
        //}
        void OnMapDataReceived( MapData mapData)
        {
            this.mapData = mapData;
            mapDataReceived = true;

            Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;
            UpdateTerrainChunk();
           // mapGenerator.RequestMeshData(mapData, OnMeshDataReceived);
        }
        public void UpdateTerrainChunk()
        {
            if (mapDataReceived)
            {

                float viewrDstFromNearestEnge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                bool visible = viewrDstFromNearestEnge <= maxViewDist;
                if (visible)
                {
                    int lodIndex = 0;

                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if (viewrDstFromNearestEnge > detailLevels[i].visibleDstThreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        } 
                    }
                    if (lodIndex != previousLODIndex)
                    {

                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if (lodMesh.hashMesh)
                        {
                            previousLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hashRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }
                    terrainChunksVisbleLastUpdate.Add(this);
                }

                SetVisible(visible);
            }
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
    class LODMesh
    {
        public Mesh mesh;
        public bool hashRequestedMesh;
        public bool hashMesh;
        int lod;
        Action updateCallback; 
        public LODMesh(int lod, Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }
        void OnMeshDataReceived(MeshData meshData)
        {
            mesh = meshData.CreatMesh();
            hashMesh = true;

            updateCallback();
        }
        public void RequestMesh(MapData mapData)
        {
            hashRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData,lod, OnMeshDataReceived);
        }
    }
    [Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDstThreshold;
    }
}
