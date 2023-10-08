using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private Transform realCamera;
    [SerializeField]
    private Transform fakeCamera;
    
    void LateUpdate()
    {

        fakeCamera.localPosition = Vector3.Lerp(fakeCamera.localPosition, -realCamera.localPosition, 0.03f);
        fakeCamera.localRotation = Quaternion.Lerp(fakeCamera.localRotation, Quaternion.Inverse(realCamera.localRotation), 0.03f);
    }
    public void ShakeCamera(float duration, float strength = 1, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true){
        realCamera.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
        
    }
    public void ShakeCamera(float duration, Vector3 strength, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true){
        realCamera.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
        
    }
    public void ShakeCameraRotation(float duration, float strength = 90, int vibrato = 10, float randomness = 90, bool fadeOut = true){
        realCamera.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut);

    }
    public void ShakeCameraRotation(float duration, Vector3 strength, int vibrato = 10, float randomness = 90, bool fadeOut = true){
        realCamera.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut);

    }
    public void ShakeCamera(float duration = 0.3f, float strength = 30, int vibrato = 10, float randomness = 90, bool fadeOut = true){
        transform.DOShakeRotation(duration, strength, vibrato, randomness, true);
    }
}
