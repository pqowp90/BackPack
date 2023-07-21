using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ConveyorVeltMesh))]
public class ConveyorVeltMeshEditor : Editor
{
    ConveyorVeltMesh conveyorVeltMesh;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Nodes"))
        {
            conveyorVeltMesh = (ConveyorVeltMesh)target;
            conveyorVeltMesh.Create();
        }
        if(conveyorVeltMesh)
        {
            conveyorVeltMesh = (ConveyorVeltMesh)target;
            conveyorVeltMesh.Create();
        }
    }
}

[RequireComponent(typeof(MeshFilter))]
public class ConveyorVeltMesh : MonoBehaviour
{
    Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> verticesBottom = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector3> veltLineVectos = new List<Vector3>();
    [SerializeField]
    private float width;
    [SerializeField]
    private float height;


    [SerializeField]
    public float Y;

    // Start is called before the first frame update
    public void Create()
    {
        //MakeMeshData();
    }

    private void CreateMesh()
    {
        if(mesh == null)
            mesh = GetComponent<MeshFilter>().sharedMesh;
        //mesh.Clear();

        mesh.RecalculateNormals();
        
    }

    public void MakeMeshData(List<Vector3> points, Vector3 start, Vector3 end)
    {
        if(points.Count < 2)
            return;

        vertices.Clear();
        verticesBottom.Clear();
        triangles.Clear();
        veltLineVectos = points;

        Quaternion forward = Quaternion.LookRotation(veltLineVectos[0] - start, Vector3.up);

        for (int i = 0; i < veltLineVectos.Count-1; i++)
        {
            vertices.Add(veltLineVectos[i] + forward * (Vector3.right * width * 0.5f));
            vertices.Add(veltLineVectos[i] + forward * (Vector3.right * width * -0.5f));
            verticesBottom.Add(veltLineVectos[i] + forward * (Vector3.right * width * 0.5f) + Vector3.down * height);
            verticesBottom.Add(veltLineVectos[i] + forward * (Vector3.right * width * -0.5f) + Vector3.down * height);
            forward = Quaternion.LookRotation(veltLineVectos[i + 1] - veltLineVectos[i], Vector3.up);
        }

        forward = Quaternion.LookRotation(end - veltLineVectos[veltLineVectos.Count-1], Vector3.up);
        
        vertices.Add(veltLineVectos[veltLineVectos.Count-1] + forward * (Vector3.right * width * 0.5f));
        vertices.Add(veltLineVectos[veltLineVectos.Count-1] + forward * (Vector3.right * width * -0.5f));
        verticesBottom.Add(veltLineVectos[veltLineVectos.Count-1] + forward * (Vector3.right * width * 0.5f) + Vector3.down * height);
        verticesBottom.Add(veltLineVectos[veltLineVectos.Count-1] + forward * (Vector3.right * width * -0.5f) + Vector3.down * height);
        
        triangles.Add(0);
        triangles.Add(vertices.Count);
        triangles.Add(1);
        triangles.Add(1);
        triangles.Add(vertices.Count);
        triangles.Add(vertices.Count + 1);

        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        triangles.Add(vertices.Count + verticesBottom.Count - 2);
        triangles.Add(vertices.Count + verticesBottom.Count - 2);
        triangles.Add(vertices.Count - 1);
        triangles.Add(vertices.Count + verticesBottom.Count - 1);

        

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
            triangles.Add(vertices.Count + i-3);
            triangles.Add(i-3);
            triangles.Add(vertices.Count + i-1);
            triangles.Add(vertices.Count + i-1);
            triangles.Add(i-3);
            triangles.Add(i-1);
        }

        for (int i = 1; i < vertices.Count; i+=2)
        {
            if(i<3) continue;
            triangles.Add(vertices.Count + i-2);
            triangles.Add(vertices.Count + i);
            triangles.Add(i-2);
            triangles.Add(i-2);
            triangles.Add(vertices.Count + i);
            triangles.Add(i);
        }

        
        if(mesh == null)
            mesh = GetComponent<MeshFilter>().sharedMesh;
        if(mesh == null) return;
        mesh.Clear();
        vertices.AddRange(verticesBottom);
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        
        CreateMesh();
        // vertices = new Vector3[] {new Vector3(0,0,0), new Vector3(0,0,1), new Vector3(1,0,0), new Vector3(1,-1,1)};
        // triangles = new int[]{0, 1, 2, 2, 1, 3, 2, 3 ,0, 3, 1, 0};
    }
    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Gizmos.color = Color.green;
        foreach (var item in vertices)
        {
            Gizmos.DrawWireSphere(item, 0.07f);
        }

        
#endif
    }
    // Update is called once per frame
    void Update()
    {
        Create();
    }
}
