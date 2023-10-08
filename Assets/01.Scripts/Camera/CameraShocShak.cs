using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ForceModeEnum{
    Walk,
    Run,
    Jump,
    Jump2,
    Land,
    Walk2,
    Run2,
    FootShake,
    FootShake2,
}
public enum ShakeModeEnum{
    None,
    V,
    F,
}
[System.Serializable]
public class Force_Mode{
    [SerializeField]
    public ForceModeEnum forceModeEnum;
    public Vector3 posVelocity;
    public Vector3 angleVelocity;
    public bool isShake = false;
    public bool isRotate = false;

    
    [Header("Presets")]
    public ShakeModeEnum shakeModeEnum = ShakeModeEnum.None;
    public float duration;
    [DrawIf("shakeModeEnum", ShakeModeEnum.V)] public Vector3 strengthV;
    [DrawIf("shakeModeEnum", ShakeModeEnum.F)]public float strengthF = 1;
    public int vibrato = 10;
    public float randomness = 90;
    public bool snapping = false;
    public bool fadeOut = true;
}

public class CameraShocShak : MonoBehaviour
{
    [SerializeField]
    private Vector3 nowPos = Vector3.zero;          // 현재 좌표
    [SerializeField]
    private Vector3 nowAngle = Vector3.zero;        // 현재 각도
    [SerializeField]
    private Vector3 cameraPosition = Vector3.zero;  // 좌표 힘
    [SerializeField]
    private Vector3 cameraAngle = Vector3.zero;   // 각도 힘
    private Vector3 realCameraPosition = Vector3.zero;  // 좌표 힘
    private Vector3 realCameraAngle = Vector3.zero;   // 각도 힘
    [SerializeField]
    private float posForceDemp = 0f;        // 좌표 힘을 가하는 속도
    [SerializeField]
    private float angleForceDemp = 0f;        // 각도 힘을 가하는 속도
    [SerializeField]
    private float positionElasticForce = 0f;        // 좌표 탄성
    [SerializeField]
    private float forceResistance = 0f;             // 좌표 힘 저항
    [SerializeField]
    private float angleElasticForce = 0f;           // 각도 탄성
    [SerializeField]
    private List<Force_Mode> shakeModes = new List<Force_Mode>(0);
    [SerializeField]
    private Transform targetTransfrom = null;
    private CameraShake cameraShake = null;
    private Animator animator;
    private void Start() {
        cameraShake = FindObjectOfType<CameraShake>();
        animator = GetComponent<Animator>();
    }
    
    
    
    private void Update() {
        if(!targetTransfrom) return;
        realCameraPosition = Vector3.Lerp(realCameraPosition, cameraPosition, posForceDemp * Time.deltaTime);
        realCameraAngle = Vector3.Lerp(realCameraAngle, cameraAngle, angleForceDemp * Time.deltaTime);

        nowPos += realCameraPosition * Time.deltaTime;
        nowAngle += realCameraAngle * Time.deltaTime * 100f;

        cameraPosition = Vector3.Lerp(cameraPosition, Vector3.zero, forceResistance * Time.deltaTime);
        cameraAngle = Vector3.Lerp(cameraAngle, Vector3.zero, forceResistance * Time.deltaTime);

        nowPos = Spring(nowPos, Vector3.zero, positionElasticForce * Time.deltaTime);
        nowAngle = Spring(nowAngle, Vector3.zero, angleElasticForce * Time.deltaTime);

        targetTransfrom.localPosition = nowPos;
        targetTransfrom.localRotation = Quaternion.Euler(nowAngle);
    }
    public void AddForce(ForceModeEnum _forceModeEnum){
        if(animator.GetFloat("WalkSpeed")<2.8f&&_forceModeEnum == ForceModeEnum.Walk)return;
        
        Force_Mode shakeMode = shakeModes.Find(x=> x.forceModeEnum == _forceModeEnum);
        if(shakeMode == null) return;
        cameraPosition += shakeMode.posVelocity;
        cameraAngle += shakeMode.angleVelocity;
        if(!shakeMode.isRotate){
            if(shakeMode.shakeModeEnum == ShakeModeEnum.V)
                cameraShake.ShakeCamera(shakeMode.duration, shakeMode.strengthV, shakeMode.vibrato, shakeMode.randomness, shakeMode.snapping, shakeMode.fadeOut);
            else if(shakeMode.shakeModeEnum == ShakeModeEnum.F)
                cameraShake.ShakeCamera(shakeMode.duration, shakeMode.strengthF, shakeMode.vibrato, shakeMode.randomness, shakeMode.snapping, shakeMode.fadeOut);
        }else{
            if(shakeMode.shakeModeEnum == ShakeModeEnum.V)
                cameraShake.ShakeCameraRotation(shakeMode.duration, shakeMode.strengthV, shakeMode.vibrato, shakeMode.randomness, shakeMode.fadeOut);
            else if(shakeMode.shakeModeEnum == ShakeModeEnum.F)
                cameraShake.ShakeCameraRotation(shakeMode.duration, shakeMode.strengthF, shakeMode.vibrato, shakeMode.randomness, shakeMode.fadeOut);
        }
    }
    
    public static float Spring(float from, float to, float time)
    {
        time = Mathf.Clamp01(time);
        time = (Mathf.Sin(time * Mathf.PI * (.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
        return from + (to - from) * time;
    }
    public static Vector3 Spring(Vector3 from, Vector3 to, float time)
    {
        return new Vector3(Spring(from.x, to.x, time), Spring(from.y, to.y, time), Spring(from.z, to.z, time));
    }
    
}
