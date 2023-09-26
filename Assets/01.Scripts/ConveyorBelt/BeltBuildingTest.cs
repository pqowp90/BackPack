using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeltBuildingTest : MonoBehaviour
{
    public PlayerTool playerTool;
    [SerializeField]
    private GameObject beltPolesPrefab;
    [SerializeField]
    private GameObject previewBeltPrefab;
    [SerializeField]
    private GameObject previewObject = null;
    private Quaternion beltRotate = Quaternion.identity; 
    [SerializeField]
    private Transform beltJoint1;
    [SerializeField]
    private Transform beltJoint2;
    [SerializeField]
    private Transform beltJoint3;
    [SerializeField]
    private Transform beltJoint4;
    private Vector3 beltPos = Vector3.zero;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private LayerMask Magnet;
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
    private ConveyorBeltMesh conveyorBeltMesh;
    private Vector3 check1;
    private Vector3 check2;
    private GameObject curObject;
    private BeltConnection curBeltConnection;
    private enum InstallationStatus
    {
        None,
        SelectFirstPoint,
        SelectSecondPoint,
        SelectHeight,
    }
    private InstallationStatus nowInstallationStat;

    private const float StandardDistance = 1;

    private Camera mainCamera;
    private Vector3 bottomPos;
    // test --------------------------------------------------------------
    public TextMeshProUGUI curStateUI;

    private void Start()
    {
        mainCamera = BuildingManager.Instance.FirstCamera;
        playerTool = GetComponent<PlayerTool>();
    }
    private void Update() {
        
        Building();
        curStateUI.text = nowInstallationStat.ToString();

        if(!beltJoint1 || !beltJoint2 || !beltJoint3 || !beltJoint4 )
        {
            return;
        }
        
        
        MakeMesh();
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
        beltJoint2.rotation = Quaternion.LookRotation(-test3V, Vector3.up);

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
        beltJoint2.rotation = Quaternion.Euler(new Vector3(0f, angle - 180f, 0f));
        
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
        beltJoint4.position = beltJoint2.right * StandardDistance/2f * ((check2.x < 0)?-StandardDistance:StandardDistance) + beltJoint2.position + beltJoint2.forward * StandardDistance/2f;
        beltJoint4.localEulerAngles = new Vector3(0f, ((check2.x < 0)?-90f:90f), 0f);

        beltJoint3.position = beltJoint1.right * StandardDistance/2f * ((check1.x < 0)?-StandardDistance:StandardDistance) + beltJoint1.position + beltJoint1.forward * StandardDistance/2f;
        beltJoint3.localEulerAngles = new Vector3(0f, ((check1.x < 0)?-90f:90f), 0f);
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
            check1 = CheckAngleIsTooBig(beltJoint1, beltJoint2);
            if(check1.z < 0)
            {
                beltJoint3.position = beltJoint1.right * StandardDistance/2f * ((check1.x < 0)?-StandardDistance:StandardDistance) + beltJoint1.position + beltJoint1.forward * StandardDistance/2f;
                beltJoint3.localEulerAngles = new Vector3(0f, ((check1.x < 0)?-90f:90f), 0f);
                beltJoint3.gameObject.SetActive(true);

                list1 = Test(beltJoint1, beltJoint3.position);
                list3 = Test(beltJoint3, beltJoint2.position);

            }
            else
            {
                beltJoint3.gameObject.SetActive(false);

                list1 = Test(beltJoint1, beltJoint2.position);
            }
        }
        else
        {
            check1 = CheckAngleIsTooBig(beltJoint1, beltJoint2);
            check2 = CheckAngleIsTooBig(beltJoint2, beltJoint1);
            SetSubJoint();

            
            if(check2.z < 0)
            {
                beltJoint4.position = beltJoint2.right * StandardDistance/2f * ((check2.x < 0)?-StandardDistance:StandardDistance) + beltJoint2.position + beltJoint2.forward * StandardDistance/2f;
                beltJoint4.localEulerAngles = new Vector3(0f, ((check2.x < 0)?-90f:90f), 0f);
                beltJoint4.gameObject.SetActive(true);
                if(check1.z < 0)
                {
                    list1 = Test2(beltJoint1, beltJoint2);
                    list4 = Test2(beltJoint4, beltJoint3);
                }
                else
                {
                    list1 = Test2(beltJoint1, beltJoint4);
                    list4 = Test2(beltJoint4, beltJoint1);
                }
            }
            else
            {
                beltJoint4.gameObject.SetActive(false);
                list1 = Test2(beltJoint1, beltJoint2);
            }
            
            if(check1.z < 0)
            {
                beltJoint3.position = beltJoint1.right * StandardDistance/2f * ((check1.x < 0)?-StandardDistance:StandardDistance) + beltJoint1.position + beltJoint1.forward * StandardDistance/2f;
                beltJoint3.localEulerAngles = new Vector3(0f, ((check1.x < 0)?-90f:90f), 0f);
                beltJoint3.gameObject.SetActive(true);
                if(check2.z < 0)
                {
                    list2 = Test2(beltJoint2, beltJoint1);
                    list3 = Test2(beltJoint3, beltJoint4);
                }
                else
                {
                    list2 = Test2(beltJoint2, beltJoint3);
                    list3 = Test2(beltJoint3, beltJoint2);
                }
            }
            else
            {
                beltJoint3.gameObject.SetActive(false);
                list2 = Test2(beltJoint2, beltJoint1);
            }

            
        }
        bool isJoint3Active = beltJoint3.gameObject.activeSelf;
        bool isJoint4Active = beltJoint4.gameObject.activeSelf;
        Vector3 check3 = CheckAngleIsTooBig(isJoint3Active?beltJoint3:beltJoint1, isJoint4Active?beltJoint4:beltJoint2);
        Vector3 check4 = CheckAngleIsTooBig(isJoint4Active?beltJoint4:beltJoint2, isJoint3Active?beltJoint3:beltJoint1);
        if(Vector3.Distance(GetCenterPos(isJoint3Active?beltJoint3:beltJoint1), GetCenterPos(isJoint4Active?beltJoint4:beltJoint2)) < StandardDistance * ((isRotated)?1f:0f))
        {
            conveyorBeltMesh.BeltForm(false);
            return;
        }
        if(((check3.z < 0 && isJoint3Active) || (check4.z < 0 && isJoint4Active)) && isRotated)
        {
            

            conveyorBeltMesh.BeltForm(false);
            return;
        }
        if(((CheckAngleIsTooBig(isJoint3Active?beltJoint3:beltJoint1, beltJoint2).z < 0)) && !isRotated)
        {
            conveyorBeltMesh.BeltForm(false);
            return;
        }
        if(angle == 0 && beltJoint1.forward == (beltJoint1.position - beltJoint2.position).normalized)
        {
            conveyorBeltMesh.BeltForm(false);
            return;
        }
        conveyorBeltMesh.BeltForm(true);
    }
    private Vector3 GetCenterPos(Transform transform)
    {
        return transform.position + transform.forward * StandardDistance/2f;
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.blue;
        foreach (var item in nomalizedPoints)
        {
            Gizmos.DrawWireSphere(item + Vector3.up * conveyorBeltMesh.height, 0.07f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(nomalizedPoints[(int)time] + Vector3.up * conveyorBeltMesh.height, 0.07f);

        
        
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

        nomalizedPoints.Add(beltJoint1.position);
        list.Add(beltJoint2.position);

        float remainDistance = 0;
        Vector3 beforeJoint = beltJoint1.position;
        foreach (var joint in list)
        {
            remainDistance += Vector3.Distance(beforeJoint, joint);
            for (int i = 1; i <= (int)(remainDistance / vertexDistance); i++)
            {
                if(Vector3.Distance(beforeJoint, beltJoint2.position) >= vertexDistance / 2f)
                    nomalizedPoints.Add(Vector3.Lerp(beforeJoint, joint, ((vertexDistance * i) / remainDistance)));
            }
            remainDistance = remainDistance % vertexDistance;
            beforeJoint = joint;
        }
        nomalizedPoints.Add(beltJoint2.position);
        
        if(nomalizedPoints.Count-1 < time)
        {
            time = 0f;
        }
        time += 0.1f;

        
        conveyorBeltMesh.MakeMeshData(nomalizedPoints, -beltJoint1.forward * StandardDistance/2f + beltJoint1.position, -beltJoint2.forward * StandardDistance/2f + beltJoint2.position);
    }
    
    float time = 0f;
    private void Building()
    {
        if(!playerTool)
        {
            playerTool = GameManager.Instance.Player.GetComponent<PlayerTool>();
        }
        if(playerTool.curToolEnum == ToolEmum.Scanner)
        {
            if(nowInstallationStat == InstallationStatus.None)
                nowInstallationStat = InstallationStatus.SelectFirstPoint;
        }
        else
        {
            nowInstallationStat = InstallationStatus.None;
        }
        BeltUpdate(nowInstallationStat);
    }
    private void BeltUpdate(InstallationStatus installationStatus)
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
            case InstallationStatus.SelectHeight:
            SelectHeightUpdate();
            break;

        }
    }
    private void NoneUpdate()
    {
        conveyorBeltMesh.ShowPreview = false;
        if(previewObject != null)
        {
            
            PoolManager.Destroy(previewObject);
            previewObject = null;
        }
    }
    private void SelectFirstPointUpdate()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit magnetRay;

        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.blue, 0.0f);

        if (Physics.SphereCast(ray, 0.5f, out magnetRay, 1000, Magnet))
        {
            curBeltConnection = magnetRay.collider.GetComponent<BeltConnection>();
            if(!curBeltConnection) return;
            conveyorBeltMesh.ShowPreview = true;
            
            
            beltPos = curBeltConnection.transform.position;

            
            
            beltJoint1.position = beltPos;
            beltJoint1.rotation = curBeltConnection.transform.rotation;

            beltJoint2.position = beltJoint1.forward * 1.1f + beltJoint1.position;
            isRotated = false;


            if(Input.GetKeyDown(KeyCode.Mouse0) && conveyorBeltMesh.goodBelt)
            {
                nowInstallationStat = InstallationStatus.SelectSecondPoint;
                conveyorBeltMesh.ShowPreview = false;
            }
        }
        else
        {
            conveyorBeltMesh.ShowPreview = false;
        }
        curBeltConnection = null;
        
    }
    // 0.3123f
    private void SelectSecondPointUpdate()
    {
        beltRotate *= Quaternion.Euler(0, Input.GetAxis("Mouse ScrollWheel") * 150f, 0);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            isRotated = true;
        }

        if (Physics.Raycast(ray, out hit, 10000, layerMask))
        {
            conveyorBeltMesh.ShowPreview = true;
            if(!previewObject)
            {
                previewObject = PoolManager.Instantiate(previewBeltPrefab);
                previewObject.transform.position = beltPos;
            }
            beltPos = hit.point;
            Vector3 pos = Vector3.Lerp(previewObject.transform.position, beltPos, Time.deltaTime*17f);
            previewObject.transform.position = pos;
            beltJoint2.position = pos + Vector3.up * 0.3123f;
            
            if(isRotated)
            {
                previewObject.transform.rotation = beltRotate;
                angle = beltRotate.eulerAngles.y;
            }
            else
            {
                previewObject.transform.rotation = beltJoint2.rotation;
            }
            
            
        }
        else
        {
            conveyorBeltMesh.ShowPreview = true;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) && conveyorBeltMesh.goodBelt)
        {
            curObject = PoolManager.Instantiate(beltPolesPrefab);
            curBeltConnection = curObject.GetComponentInChildren<BeltConnection>();
            curObject.transform.position = previewObject.transform.position;
            curObject.transform.rotation = previewObject.transform.rotation;
            bottomPos = curObject.transform.position;
            curBeltConnection.SetPos(bottomPos);
            nowInstallationStat = InstallationStatus.SelectHeight;
            
            PoolManager.Destroy(previewObject);
            previewObject = null;
        }
    }
    private void SelectHeightUpdate()
    {
        float distance = Vector3.Distance(bottomPos, mainCamera.transform.position);
        float height = mainCamera.transform.position.y - bottomPos.y;
        float angle = mainCamera.transform.localEulerAngles.x * Mathf.Deg2Rad;

        float targetHeight = Mathf.Clamp(Mathf.Tan(-angle) * distance + height, 0, Mathf.Infinity);

        curObject.transform.position = bottomPos + Vector3.up * targetHeight;
        beltJoint2.position = curObject.transform.position + Vector3.up * 0.3123f;

        curBeltConnection.SetHeight(targetHeight);
        

        if(Input.GetKeyDown(KeyCode.Mouse0) && conveyorBeltMesh.goodBelt)
        {
            nowInstallationStat = InstallationStatus.None;
        }
    }

}
