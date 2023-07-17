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
    [SerializeField]
    private Transform test3;
    [SerializeField]
    private Transform test4;
    private Vector3 veltPos = Vector3.zero;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private int pointByAngle = 10;
    [SerializeField]
    private bool isRotated = false;
    [SerializeField]
    private float angle = 0f;
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
    private void Test(Transform target1, Vector3 target2V)
    {
        if(!target1)
            return;
        var test1V = target1.position;
        var test2V = target2V;
 
        var test3V = test2V - (test1V + target1.forward * 0.5f);
        test3V.y = 0f;

        test3V = Quaternion.Inverse(target1.rotation) * test3V;
        if(test3V.z < 0 )
        {
            test3V.z = 0f;
        }
        test3V = target1.rotation * test3V;

        var test4V = Vector3.ClampMagnitude(test3V, 0.5f);

        
        if(test4V.magnitude < 0.1f)
        {
            list.Add(test1V);
            list.Add(test2V);
            return;
        }


        int count = pointByAngle/2 + (int)(pointByAngle * test4V.magnitude * 0.5f);

        for (int i = 0; i <= count; i++)
        {
            list.Add(BezierCurve.QuadraticCurve(test1V, test1V + target1.forward * 0.5f, test1V + (target1.forward + test3V.normalized) * 0.5f , 
                ((float)i / (float)count) * (test4V.magnitude + 0.5f)));
        }
        test2.rotation = Quaternion.LookRotation(-test3V, Vector3.up);
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
        
        Debug.DrawLine(test1V + target1.forward * 0.5f, test1V + (target1.forward + test3V.normalized) * 0.5f , Color.blue, 0f);
        // Debug.DrawLine(test2V + target2.forward * 0.5f, test4V, Color.blue, 0f);
    }
    private void Test3(Transform target1, Transform target2)
    {
        if(!target1 || !target2)
            return;
        var test1V = target1.position;
        var test2V = target2.position + target2.forward * 0.5f;
 
        var test3V = test2V - (test1V + target1.forward * 0.5f);
        test3V.y = 0f;

        test3V = Quaternion.Inverse(target1.rotation) * test3V;
        if(test3V.z < 0 )
        {
            test3V.z = 0f;
        }
        else{

        }
        test3V = target1.rotation * test3V;

        var test4V = Vector3.ClampMagnitude(test3V, 0.5f);

        



        int count = pointByAngle/2 + (int)(pointByAngle * test4V.magnitude * 0.5f);

        for (int i = 0; i <= count; i++)
        {
            list.Add(BezierCurve.QuadraticCurve(test1V, test1V + target1.forward * 0.5f, test1V + (target1.forward + test3V.normalized) * 0.5f , 
                ((float)i / (float)count) * (test4V.magnitude + 0.5f)));
        }
        test2.rotation = Quaternion.Euler(new Vector3(0f, angle - 180f, 0f));
        
        Debug.DrawLine(test1V + target1.forward * 0.5f, test1V + (target1.forward + test3V.normalized) * 0.5f , Color.blue, 0f);
    }
    private void Test2(Transform target1, Transform target2)
    {
        if(!target1 || !target2)
            return;
        var test1V = target1.position;
        var test2V = target2.position + target2.forward * 0.5f;
 
        var test3V = test2V - (test1V + target1.forward * 0.5f);
        test3V.y = 0f;

        var test4V = Vector3.ClampMagnitude(test3V, 0.5f);

        test3V = Quaternion.Inverse(target1.rotation) * test3V;
        if(test3V.z < 0 )
        {
            test3V.z = 0f;
        }

        test3V = target1.rotation * test3V;


        



        int count = pointByAngle/2 + (int)(pointByAngle * test4V.magnitude * 0.5f);

        for (int i = 0; i <= count; i++)
        {
            list.Add(BezierCurve.QuadraticCurve(test1V, test1V + target1.forward * 0.5f, test1V + (target1.forward + test3V.normalized) * 0.5f , 
                ((float)i / (float)count) * (test4V.magnitude + 0.5f)));
        }
        test2.rotation = Quaternion.Euler(new Vector3(0f, angle - 180f, 0f));
        
        Debug.DrawLine(test1V + target1.forward * 0.5f, test1V + (target1.forward + test3V.normalized) * 0.5f , Color.blue, 0f);
    }
    private Vector3 CheckAngleIsTooBig(Transform target1, Transform target2)
    {
        var test3V = (target2.position + target2.forward * 0.5f) - (target1.position + target1.forward * 0.5f);
        test3V.y = 0f;

        test3V = Quaternion.Inverse(target1.rotation) * test3V;

        return test3V;
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        list.Clear();
        if(!isRotated)
        {
            Test(test1, test2.position);
        }
        else
        {
            Vector3 check1 = CheckAngleIsTooBig(test1, test2);
            Vector3 check2 = CheckAngleIsTooBig(test2, test1);
            if(check2.z < 0)
            {
                test4.gameObject.SetActive(true);
                test4.position = test2.right * 0.5f * ((check2.x < 0)?-1f:1f) + test2.position + test2.forward * 0.5f;
                test4.localEulerAngles = new Vector3(0f, ((check2.x < 0)?-90f:90f), 0f);
                Test2(test1, test4);
            }
            else
            {
                test4.gameObject.SetActive(false);
                Test2(test1, test2);
            }
            
            if(check1.z < 0)
            {
                test3.gameObject.SetActive(true);
                test3.position = test1.right * 0.5f * ((check1.x < 0)?-1f:1f) + test1.position + test1.forward * 0.5f;
                test3.localEulerAngles = new Vector3(0f, ((check1.x < 0)?-90f:90f), 0f);
                Test2(test2, test3);
            }
            else
            {
                test3.gameObject.SetActive(false);
                Test2(test2, test1);
            }



            
            

            
           
        }
        
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
