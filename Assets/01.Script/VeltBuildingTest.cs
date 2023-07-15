using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeltBuildingTest : MonoBehaviour
{
    public PlayerTool playerTool;
    public Camera mainCamera;
    public GameObject beltPolesPrefab;
    public GameObject previewBeltPrefab;
    public GameObject previewObject;
    private Quaternion veltRotate = Quaternion.identity; 
    [SerializeField]
    private Transform test1;
    [SerializeField]
    private Transform test2;
    private Vector3 veltPos = Vector3.zero;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private List<Vector3> list = new List<Vector3>();
    private void Start()
    {
        mainCamera = Camera.main;
        playerTool = GetComponent<PlayerTool>();
    }
    private void Update() {
        
        Building();
    }
    private void Test()
    {
        if(!test1 || !test2)
            return;
        var test1V = test1.position - test1.forward * 0.5f;
        var test2V = test2.position - test2.forward * 0.5f;
        var test3V = (test2.position - test1.position).normalized * 0.5f + test1.position;
        var test4V = (test1.position - test2.position).normalized * 0.5f + test2.position;
        list.Clear();
        for (int i = 0; i < 11; ++i)
        {
            list.Add(BezierCurve.QuadraticCurve(test1V, test1V + test1.forward * 0.5f, test3V, (float)i / 10f));
            list.Add(BezierCurve.QuadraticCurve(test2V, test2V + test2.forward * 0.5f, test4V, (float)i / 10f));
        }
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Test();
        Gizmos.color = Color.red;
        foreach (var item in list)
        {
            Gizmos.DrawWireSphere(item, 0.1f);
        }
#endif
    }
    private void Building()
    {
        if(playerTool.curToolEnum == ToolEmum.Scanner)
        {
            veltRotate *= Quaternion.Euler(0, Input.GetAxis("Mouse ScrollWheel") * 150f, 0);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000, layerMask))
            {
                if(!previewObject)
                {
                    previewObject = PoolManager.Instantiate(previewBeltPrefab);
                }
                veltPos = hit.point;
                previewObject.transform.position = Vector3.Lerp(previewObject.transform.position, veltPos, Time.deltaTime*17f);
                previewObject.transform.rotation = veltRotate;
                
            }
        }
    }
}
