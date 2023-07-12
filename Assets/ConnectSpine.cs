using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectSpine : MonoBehaviour
{
    private Transform child;
    private Transform childchild;
    private Vector3 offset;
    private Vector3 offsetoffset;
    private Quaternion rotateOffset;
    private void Awake() {
        child = transform.GetChild(0);
        childchild = child.GetChild(0);
        offset = child.localPosition;

        rotateOffset = childchild.localRotation;
    }
    void Update()
    {
        transform.localPosition = -child.localPosition + offset;
        transform.localRotation = Quaternion.Inverse(childchild.localRotation) * rotateOffset;
    }
}

