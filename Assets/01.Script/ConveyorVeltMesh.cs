using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ConveyorVeltMesh : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    private void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        MakeMeshData();
        CreateMesh();
    }

    private void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
    }

    private void MakeMeshData()
    {
        vertices = new Vector3[] {new Vector3(0,0,0), new Vector3(0,0,1), new Vector3(1,0,0)};
        triangles = new int[]{0, 1, 2};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
