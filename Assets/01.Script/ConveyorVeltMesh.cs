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
    private List<Vector3> vertices;
    private List<int> triangles;


    [SerializeField]
    public float Y;

    // Start is called before the first frame update
    public void Create()
    {
        //MakeMeshData();
        CreateMesh();
    }

    private void CreateMesh()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();

        mesh.RecalculateNormals();
        
    }

    private void MakeMeshData(List<Vector3> points)
    {
        foreach (var point in points)
        {
            vertices.Add(point);
        }
        
        // vertices = new Vector3[] {new Vector3(0,0,0), new Vector3(0,0,1), new Vector3(1,0,0), new Vector3(1,-1,1)};
        // triangles = new int[]{0, 1, 2, 2, 1, 3, 2, 3 ,0, 3, 1, 0};
    }

    // Update is called once per frame
    void Update()
    {
        Create();
    }
}
