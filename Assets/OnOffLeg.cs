using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffLeg : MonoBehaviour
{
    [SerializeField] public Transform CameraTransform;
    [SerializeField] public GameObject LegGameObject;
    void Update()
    {
        LegGameObject.SetActive(CameraTransform.localRotation.x > 0);
    }
}
