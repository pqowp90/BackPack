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
    private int pointByAngle = 10;
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
    private void Test(Transform target1, Transform target2)
    {
        if(!target1 || !test2)
            return;
        var test1V = target1.position;
        var test2V = target2.position;
        var test3V = test2V - (test1V + test1.forward * 0.5f);
        test3V.y = 0f;

        var test4V = Vector3.ClampMagnitude(test3V, 0.5f);
        list.Clear();

        for (int i = 0; i <= pointByAngle; i++)
        {
            list.Add(BezierCurve.QuadraticCurve(test1V, test1V + target1.forward * 0.5f, test1V + (test1.forward + test3V.normalized) * 0.5f , 
                ((float)i / (float)pointByAngle) * (test4V.magnitude + 0.5f)));
        }
        // var test1V = target1.position;
        // var test2V = target2.position;
        // Vector3 targetDir = ((test2V + target2.forward * 0.5f) - (test1V + target1.forward * 0.5f));
        // Vector3 targetDir2 = ((test1V + target1.forward * 0.5f) - (test2V + target2.forward * 0.5f));
        // var test3V = targetDir.normalized * 0.5f + test1V + target1.forward * 0.5f;
        // var test4V = targetDir2.normalized * 0.5f + test2V + target2.forward * 0.5f;
        // list.Clear();
        // for (int i = 0; i <= pointByAngle; ++i)
        // {
        //     list.Add(BezierCurve.QuadraticCurve(test1V, test1V + target1.forward * 0.5f, test3V, 
        //     ((float)i / (float)pointByAngle) * Mathf.Clamp(targetDir.magnitude, -1f, 1f)));
        // }
        
        // for (int i = 0; i <= pointByAngle; ++i)
        // {
        //     list.Add(BezierCurve.QuadraticCurve(test2V, test2V + target2.forward * 0.5f, test4V, 
        //     ((float)i / (float)pointByAngle) * Mathf.Clamp(targetDir2.magnitude, -1f, 1f)));
        // }
        
        // Debug.DrawLine(test1V + target1.forward * 0.5f, test3V, Color.blue, 0f);
        // Debug.DrawLine(test2V + target2.forward * 0.5f, test4V, Color.blue, 0f);
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Test(test1, test2);
        Gizmos.color = Color.red;
        foreach (var item in list)
        {
            Gizmos.DrawWireSphere(item, 0.07f);
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
