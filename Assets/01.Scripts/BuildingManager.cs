using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BuildingManager : MonoSingleton<BuildingManager>
{
    private Camera firstCamera;
    public Camera FirstCamera => firstCamera;
    private void Awake() {
        firstCamera = Camera.main;
    }
    public void SetCamera(Camera camera)
    {
        firstCamera = camera;
    }
    private void Update() {
        
    }

}

