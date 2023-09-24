using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VeltBuildingTest : MonoBehaviour
{
    public PlayerTool playerTool;
    public Camera mainCamera;
    [SerializeField]
    private GameObject beltPolesPrefab;
    [SerializeField]
    private GameObject previewBeltPrefab;
    private GameObject previewObject;
    private Quaternion veltRotate = Quaternion.identity; 
    [SerializeField]
    private Transform veltJoint1;
    [SerializeField]
    private Transform veltJoint2;
    [SerializeField]
    private Transform veltJoint3;
    [SerializeField]
    private Transform veltJoint4;
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
    [SerializeField]
    private float vertexDistance = 0.1f;
    private List<Vector3> list1 = new List<Vector3>();
    private List<Vector3> list2 = new List<Vector3>();
    private List<Vector3> list3 = new List<Vector3>();
    private List<Vector3> list4 = new List<Vector3>();
    [SerializeField]
    private List<Vector3> nomalizedPoints = new List<Vector3>();
    [SerializeField]
    private ConveyorVeltMesh conveyorVeltMesh;
    private Vector3 check1;
    private Vector3 check2;
    private GameObject curObject;
    private enum InstallationStatus
    {
        None,
        SelectFirstPoint,
        SelectFirstHeight,
        SelectSecondPoint,
        SelectSecondHeight,
    }
    private InstallationStatus nowInstallationStat;

    private const float StandardDistance = 1;

    public TextMeshProUGUI curStateUI;

    private void Start()
    {
        mainCamera = Camera.main;
        playerTool = GetComponent<PlayerTool>();
    }
    private void Update() {
        
        Building();
        curStateUI.text = nowInstallationStat.ToString();
    }
    private List<Vector3> Test(Transform target1, Vector3 target2V)
    {
        List<Vector3> joints = new List<Vector3>();
        if(!target1)
            return null;
        var test1V = target1.position;
        var test2V = target2V;
 
        var test3V = test2V - (test1V + target1.forward * StandardDistance/2f);
        //test3V.y = 0f;

        test3V = Quaternion.Inverse(target1.rotation) * test3V;
        if(test3V.z < 0 )
        {
            test3V.z = 0f;
        }
        test3V = target1.rotation * test3V;

        var test4V = Vector3.ClampMagnitude(test3V, StandardDistance/2f);

        
        if(test4V.magnitude < vertexDistance)
        {
            joints.Add(test1V);
            joints.Add(test2V);
            return joints;
        }


        int count = pointByAngle/2 + (int)(pointByAngle * test4V.magnitude * StandardDistance/2f);

        for (int i = 0; i <= count; i++)
        {
            joints.Add(BezierCurve.QuadraticCurve(test1V, test1V + target1.forward * StandardDistance/2f, test1V + (target1.forward + test3V.normalized) * StandardDistance/2f , 
                ((float)i / (float)count) * (test4V.magnitude + StandardDistance/2f)));
        }
        veltJoint2.rotation = Quaternion.LookRotation(-test3V, Vector3.up);

        Debug.DrawLine(test1V + target1.forward * StandardDistance/2f, test1V + (target1.forward + test3V.normalized) * StandardDistance/2f , Color.blue, 0f);
        
        return joints;
    }
    
    private List<Vector3> Test2(Transform target1, Transform target2)
    {
        List<Vector3> joints = new List<Vector3>();
        if(!target1 || !target2)
            return null;
        var test1V = target1.position;
        var test2V = target2.position + target2.forward * StandardDistance/2f;
 
        var test3V = test2V - (test1V + target1.forward * StandardDistance/2f);
        test3V.y = 0f;

        var test4V = Vector3.ClampMagnitude(test3V, StandardDistance/2f);

        test3V = Quaternion.Inverse(target1.rotation) * test3V;
        if(test3V.z < 0 )
        {
            test3V.z = 0f;
        }

        test3V = target1.rotation * test3V;


        



        int count = pointByAngle/2 + (int)(pointByAngle * test4V.magnitude * StandardDistance/2f);

        for (int i = 0; i <= count; i++)
        {
            joints.Add(BezierCurve.QuadraticCurve(test1V, test1V + target1.forward * StandardDistance/2f, test1V + (target1.forward + test3V.normalized) * StandardDistance/2f , 
                ((float)i / (float)count) * (test4V.magnitude + StandardDistance/2f)));
        }
        veltJoint2.rotation = Quaternion.Euler(new Vector3(0f, angle - 180f, 0f));
        
        Debug.DrawLine(test1V + target1.forward * StandardDistance/2f, test1V + (target1.forward + test3V.normalized) * StandardDistance/2f , Color.blue, 0f);
        return joints;
    }
    private Vector3 CheckAngleIsTooBig(Transform target1, Transform target2)
    {
        var test3V = GetCenterPos(target2) - GetCenterPos(target1);
        test3V.y = 0f;

        test3V = Quaternion.Inverse(target1.rotation) * test3V;

        return test3V;
    }
    private void SetSubJoint()
    {
        veltJoint4.position = veltJoint2.right * StandardDistance/2f * ((check2.x < 0)?-StandardDistance:StandardDistance) + veltJoint2.position + veltJoint2.forward * StandardDistance/2f;
        veltJoint4.localEulerAngles = new Vector3(0f, ((check2.x < 0)?-90f:90f), 0f);

        veltJoint3.position = veltJoint1.right * StandardDistance/2f * ((check1.x < 0)?-StandardDistance:StandardDistance) + veltJoint1.position + veltJoint1.forward * StandardDistance/2f;
        veltJoint3.localEulerAngles = new Vector3(0f, ((check1.x < 0)?-90f:90f), 0f);
    }
    private void GetJoints()
    {
        list.Clear();
        list1.Clear();
        list2.Clear();
        list3.Clear();
        list4.Clear();
        if(!isRotated)
        {
            check1 = CheckAngleIsTooBig(veltJoint1, veltJoint2);
            if(check1.z < 0)
            {
                veltJoint3.position = veltJoint1.right * StandardDistance/2f * ((check1.x < 0)?-StandardDistance:StandardDistance) + veltJoint1.position + veltJoint1.forward * StandardDistance/2f;
                veltJoint3.localEulerAngles = new Vector3(0f, ((check1.x < 0)?-90f:90f), 0f);
                veltJoint3.gameObject.SetActive(true);

                list1 = Test(veltJoint1, veltJoint3.position);
                list3 = Test(veltJoint3, veltJoint2.position);

            }
            else
            {
                veltJoint3.gameObject.SetActive(false);

                list1 = Test(veltJoint1, veltJoint2.position);
            }
        }
        else
        {
            check1 = CheckAngleIsTooBig(veltJoint1, veltJoint2);
            check2 = CheckAngleIsTooBig(veltJoint2, veltJoint1);
            SetSubJoint();

            
            if(check2.z < 0)
            {
                veltJoint4.position = veltJoint2.right * StandardDistance/2f * ((check2.x < 0)?-StandardDistance:StandardDistance) + veltJoint2.position + veltJoint2.forward * StandardDistance/2f;
                veltJoint4.localEulerAngles = new Vector3(0f, ((check2.x < 0)?-90f:90f), 0f);
                veltJoint4.gameObject.SetActive(true);
                if(check1.z < 0)
                {
                    list1 = Test2(veltJoint1, veltJoint2);
                    list4 = Test2(veltJoint4, veltJoint3);
                }
                else
                {
                    list1 = Test2(veltJoint1, veltJoint4);
                    list4 = Test2(veltJoint4, veltJoint1);
                }
            }
            else
            {
                veltJoint4.gameObject.SetActive(false);
                list1 = Test2(veltJoint1, veltJoint2);
            }
            
            if(check1.z < 0)
            {
                veltJoint3.position = veltJoint1.right * StandardDistance/2f * ((check1.x < 0)?-StandardDistance:StandardDistance) + veltJoint1.position + veltJoint1.forward * StandardDistance/2f;
                veltJoint3.localEulerAngles = new Vector3(0f, ((check1.x < 0)?-90f:90f), 0f);
                veltJoint3.gameObject.SetActive(true);
                if(check2.z < 0)
                {
                    list2 = Test2(veltJoint2, veltJoint1);
                    list3 = Test2(veltJoint3, veltJoint4);
                }
                else
                {
                    list2 = Test2(veltJoint2, veltJoint3);
                    list3 = Test2(veltJoint3, veltJoint2);
                }
            }
            else
            {
                veltJoint3.gameObject.SetActive(false);
                list2 = Test2(veltJoint2, veltJoint1);
            }

            
        }
        bool isJoint3Active = veltJoint3.gameObject.activeSelf;
        bool isJoint4Active = veltJoint4.gameObject.activeSelf;
        Vector3 check3 = CheckAngleIsTooBig(isJoint3Active?veltJoint3:veltJoint1, isJoint4Active?veltJoint4:veltJoint2);
        Vector3 check4 = CheckAngleIsTooBig(isJoint4Active?veltJoint4:veltJoint2, isJoint3Active?veltJoint3:veltJoint1);
        if(Vector3.Distance(GetCenterPos(isJoint3Active?veltJoint3:veltJoint1), GetCenterPos(isJoint4Active?veltJoint4:veltJoint2)) < StandardDistance * ((isRotated)?1f:0f))
        {
            conveyorVeltMesh.VeltForm(false);
            return;
        }
        if(((check3.z < 0 && isJoint3Active) || (check4.z < 0 && isJoint4Active)) && isRotated)
        {
            

            conveyorVeltMesh.VeltForm(false);
            return;
        }
        if(((CheckAngleIsTooBig(isJoint3Active?veltJoint3:veltJoint1, veltJoint2).z < 0)) && !isRotated)
        {
            conveyorVeltMesh.VeltForm(false);
            return;
        }
        if(angle == 0 && veltJoint1.forward == (veltJoint1.position - veltJoint2.position).normalized)
        {
            conveyorVeltMesh.VeltForm(false);
            return;
        }
        conveyorVeltMesh.VeltForm(true);
    }
    private Vector3 GetCenterPos(Transform transform)
    {
        return transform.position + transform.forward * StandardDistance/2f;
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if(!veltJoint1 || !veltJoint2 || !veltJoint3 || !veltJoint4 )
        {
            return;
        }
        
        
        MakeMesh();
        
#endif
    }
    private void MakeMesh()
    {
        GetJoints();
        list.Clear();
        nomalizedPoints.Clear();
        
        list.AddRange(list1);
        list.AddRange(list3);
        list4.Reverse();
        list2.Reverse();
        list.AddRange(list4);
        list.AddRange(list2);

        nomalizedPoints.Add(veltJoint1.position);
        list.Add(veltJoint2.position);

        float remainDistance = 0;
        Vector3 beforeJoint = veltJoint1.position;
        foreach (var joint in list)
        {
            remainDistance += Vector3.Distance(beforeJoint, joint);
            for (int i = 1; i <= (int)(remainDistance / vertexDistance); i++)
            {
                if(Vector3.Distance(beforeJoint, veltJoint2.position) >= vertexDistance / 2f)
                    nomalizedPoints.Add(Vector3.Lerp(beforeJoint, joint, ((vertexDistance * i) / remainDistance)));
            }
            remainDistance = remainDistance % vertexDistance;
            beforeJoint = joint;
        }
        nomalizedPoints.Add(veltJoint2.position);
        
        if(nomalizedPoints.Count-1 < time)
        {
            time = 0f;
        }
        time += 0.1f;

        Gizmos.color = Color.blue;
        foreach (var item in nomalizedPoints)
        {
            Gizmos.DrawWireSphere(item, 0.07f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(nomalizedPoints[(int)time], 0.07f);
        conveyorVeltMesh.MakeMeshData(nomalizedPoints, -veltJoint1.forward * StandardDistance/2f + veltJoint1.position, -veltJoint2.forward * StandardDistance/2f + veltJoint2.position);
    }
    
    float time = 0f;
    private void Building()
    {
        if(playerTool.curToolEnum == ToolEmum.Scanner)
        {
            if(nowInstallationStat == InstallationStatus.None)
                nowInstallationStat = InstallationStatus.SelectFirstPoint;
        }
        else
        {
            nowInstallationStat = InstallationStatus.None;
        }
        VeltUpdate(nowInstallationStat);
    }
    private void VeltUpdate(InstallationStatus installationStatus)
    {
        switch (installationStatus)
        {
            case InstallationStatus.None:
            NoneUpdate();
            break;
            case InstallationStatus.SelectFirstPoint:
            SelectFirstPointUpdate();
            break;
            case InstallationStatus.SelectSecondPoint:
            SelectSecondPointUpdate();
            break;

        }
    }
    private void NoneUpdate()
    {
        if(previewObject != null)
        {
            
            PoolManager.Destroy(previewObject);
            previewObject = null;
        }
    }
    private void SelectFirstPointUpdate()
    {
        veltRotate *= Quaternion.Euler(0, Input.GetAxis("Mouse ScrollWheel") * 150f, 0);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, layerMask))
        {
            if(!previewObject)
            {
                previewObject = PoolManager.Instantiate(previewBeltPrefab);
                previewObject.transform.position = veltPos;
            }
            veltPos = hit.point;
            previewObject.transform.position = Vector3.Lerp(previewObject.transform.position, veltPos, Time.deltaTime*17f);
            previewObject.transform.rotation = veltRotate;
            
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            curObject = PoolManager.Instantiate(beltPolesPrefab);
            curObject.transform.position = previewObject.transform.position;
            curObject.transform.rotation = previewObject.transform.rotation;
            nowInstallationStat = InstallationStatus.SelectFirstHeight;

            PoolManager.Destroy(previewObject);
            previewObject = null;
        }
    }
    private void SelectSecondPointUpdate()
    {
        veltRotate *= Quaternion.Euler(0, Input.GetAxis("Mouse ScrollWheel") * 150f, 0);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, layerMask))
        {
            if(!previewObject)
            {
                previewObject = PoolManager.Instantiate(previewBeltPrefab);
                previewObject.transform.position = veltPos;
            }
            veltPos = hit.point;
            previewObject.transform.position = Vector3.Lerp(previewObject.transform.position, veltPos, Time.deltaTime*17f);
            previewObject.transform.rotation = veltRotate;
            
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            curObject = PoolManager.Instantiate(beltPolesPrefab);
            curObject.transform.position = previewObject.transform.position;
            curObject.transform.rotation = previewObject.transform.rotation;
            nowInstallationStat = InstallationStatus.SelectSecondHeight;
            
            PoolManager.Destroy(previewObject);
            previewObject = null;
        }
    }

}
