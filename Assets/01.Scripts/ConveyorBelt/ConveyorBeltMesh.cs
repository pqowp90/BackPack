using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(MeshFilter))]
public class ConveyorBeltMesh : MonoBehaviour
{
    private List<Vector3> beltLineVectos = new List<Vector3>();
    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> verticesBottom = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<int> triangles2 = new List<int>();
    private List<Vector2> beltUVs = new List<Vector2>();
    private List<Vector2> beltUVsBottom = new List<Vector2>();
    [SerializeField]
    private Material[] mats;
    [SerializeField]
    private Material[] mats2;
    [SerializeField]
    private float width;
    [SerializeField]
    public float height;


    [SerializeField]
    public float Y;
    public bool goodBelt;

    public bool ShowPreview {
        get
        {
            return meshRenderer.enabled;
        } 
        set
        {
            if(meshRenderer && meshRenderer.enabled != value)
                meshRenderer.enabled = value;
            if(meshCollider && meshCollider.enabled != value)
                meshCollider.enabled = value;
        }
    }

    public void Create()
    {
        CreateMesh();
        GetMyComponent();
    }

    private void CreateMesh()
    {
        if(mesh == null)
            mesh = GetComponent<MeshFilter>().mesh;
        //mesh.Clear();
        
        mesh.RecalculateNormals();
        
    }
    private void GetMyComponent()
    {
        if(meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        if(meshCollider == null){
            meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }
    }
    public void BeltForm(bool valid)
    {
        GetMyComponent();
        if(valid)
        {
            meshRenderer.sharedMaterials = mats;
        }
        else
        {
            meshRenderer.sharedMaterials = mats2;
        }
        goodBelt = valid;
    }

    public void MakeMeshData(List<Vector3> points, Vector3 start, Vector3 end)
    {
        
        GetMyComponent();
        CreateMesh();
        if(points.Count < 2)
            return;

        vertices.Clear();
        verticesBottom.Clear();
        triangles.Clear();
        triangles2.Clear();
        beltUVs.Clear();
        beltUVsBottom.Clear();
        beltLineVectos = points;

        float uvPersent = 0f;

        Quaternion forward = Quaternion.LookRotation(beltLineVectos[1] - start, Vector3.up);

        for (int i = 0; i < beltLineVectos.Count; i++)
        {
            uvPersent = i / (float)(beltLineVectos.Count-1);
            vertices.Add(beltLineVectos[i] + forward * (Vector3.right * width * 0.5f) + Vector3.up * height);
            vertices.Add(beltLineVectos[i] + forward * (Vector3.right * width * -0.5f) + Vector3.up * height);
            float uvi = i%2;
            uvi = i;
            beltUVs.Add(new Vector2(0f, uvi));
            beltUVs.Add(new Vector2(1f, uvi));
            verticesBottom.Add(beltLineVectos[i] + forward * (Vector3.right * width * 0.5f));
            verticesBottom.Add(beltLineVectos[i] + forward * (Vector3.right * width * -0.5f));
            if(i < beltLineVectos.Count-2)
                forward = Quaternion.LookRotation(beltLineVectos[i + 2] - beltLineVectos[i], Vector3.up);
            else
                forward = Quaternion.LookRotation(end - beltLineVectos[beltLineVectos.Count-1], Vector3.up);
        }

        for (int i = 1; i < vertices.Count; i+=2)
        {
            if(i<3) continue;
            triangles.Add(i-3);
            triangles.Add(i-2);
            triangles.Add(i-1);
            triangles.Add(i-1);
            triangles.Add(i-2);
            triangles.Add(i);
        }
        for (int i = 1; i < verticesBottom.Count; i+=2)
        {
            if(i<3) continue;
            triangles.Add(vertices.Count + i-3);
            triangles.Add(vertices.Count + i-1);
            triangles.Add(vertices.Count + i-2);
            triangles.Add(vertices.Count + i-2);
            triangles.Add(vertices.Count + i-1);
            triangles.Add(vertices.Count + i);

        }
        for (int i = 1; i < vertices.Count; i+=2)
        {
            if(i<3) continue;
            triangles2.Add(vertices.Count + i-3);
            triangles2.Add(i-3);
            triangles2.Add(vertices.Count + i-1);
            triangles2.Add(vertices.Count + i-1);
            triangles2.Add(i-3);
            triangles2.Add(i-1);
        }

        for (int i = 1; i < vertices.Count; i+=2)
        {
            if(i<3) continue;
            triangles2.Add(vertices.Count + i-2);
            triangles2.Add(vertices.Count + i);
            triangles2.Add(i-2);
            triangles2.Add(i-2);
            triangles2.Add(vertices.Count + i);
            triangles2.Add(i);
        }
        triangles2.Add(0);
        triangles2.Add(vertices.Count);
        triangles2.Add(1);
        triangles2.Add(1);
        triangles2.Add(vertices.Count);
        triangles2.Add(vertices.Count + 1);

        triangles2.Add(vertices.Count - 2);
        triangles2.Add(vertices.Count - 1);
        triangles2.Add(vertices.Count + verticesBottom.Count - 2);
        triangles2.Add(vertices.Count + verticesBottom.Count - 2);
        triangles2.Add(vertices.Count - 1);
        triangles2.Add(vertices.Count + verticesBottom.Count - 1);

        
        if(mesh == null)
            mesh = GetComponent<MeshFilter>().sharedMesh;
        if(mesh == null) return;
        mesh.Clear();
        vertices.AddRange(verticesBottom);
        
        
        


        mesh.vertices = vertices.ToArray();
        //mesh.triangles = triangles.ToArray();
        foreach (var uv in beltUVs)
        {
            beltUVsBottom.Add(-uv);
        }
        
        mesh.subMeshCount = 2;

        mesh.SetTriangles(triangles, 0);
        mesh.SetTriangles(triangles2, 1);

        beltUVs.AddRange(beltUVsBottom);
        
        mesh.uv = beltUVs.ToArray();
        
        //CreateMesh();
        meshCollider.sharedMesh = mesh;
        Create();
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.green;
        foreach (var item in vertices)
        {
            Gizmos.DrawWireSphere(item, 0.01f);
        }
#endif
    }
    void Update()
    {
        //Create();
    }
}
