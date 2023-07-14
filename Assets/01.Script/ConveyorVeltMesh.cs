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
        if (GUILayout.Button("Generate Nodes"))
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

    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    public void Create()
    {
        MakeMeshData();
        CreateMesh();
    }

    private void CreateMesh()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
    }

    private void MakeMeshData()
    {
        vertices = new Vector3[] {new Vector3(0,1,0), new Vector3(0,0,1), new Vector3(1,0,0)};
        triangles = new int[]{0, 1, 2};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
