using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConnection : MonoBehaviour
{
    private float height;
    [SerializeField]
    private Transform de;
    [SerializeField]
    private Transform ggDe;
    private Vector3 pos;
    public void SetHeight(float _height)
    {
        height = _height;
        de.localScale = new Vector3(0.1982441f, 0f, 0.1982441f) + Vector3.up * height;
        ggDe.localPosition = -Vector3.up * height;
    }
    public void SetPos(Vector3 _pos)
    {
        pos = _pos;
    }
}
