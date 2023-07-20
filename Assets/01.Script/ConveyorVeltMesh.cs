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
        CreateMesh();
    }

    private void CreateMesh()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();

        mesh.RecalculateNormals();
        
    }

    public void MakeMeshData(List<Vector3> points, Vector3 start, Vector3 end)
    {
        if(points.Count < 2)
            return;
        vertices.Clear();
        veltLineVectos = points;

        Quaternion forward = Quaternion.LookRotation(new Vector3(start.x, 0f, start.z), new Vector3(veltLineVectos[0].x, 0f, veltLineVectos[0].z));

        for (int i = 0; i < veltLineVectos.Count-1; i++)
        {
            vertices.Add(veltLineVectos[i] + forward * (Vector3.right * width * 0.5f));
            vertices.Add(veltLineVectos[i] + forward * (Vector3.right * width * -0.5f));
            forward = Quaternion.LookRotation(new Vector3(veltLineVectos[i].x, 0f, veltLineVectos[i].z), new Vector3(veltLineVectos[i + 1].x, 0f, veltLineVectos[i + 1].z));
        }

        forward = Quaternion.LookRotation(new Vector3(veltLineVectos[veltLineVectos.Count-1].x, 0f, veltLineVectos[veltLineVectos.Count-1].z), new Vector3(end.x, 0f, end.z));
        
        vertices.Add(veltLineVectos[veltLineVectos.Count-1] + forward * (Vector3.right * width * 0.5f));
        vertices.Add(veltLineVectos[veltLineVectos.Count-1] + forward * (Vector3.right * width * -0.5f));
        

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
