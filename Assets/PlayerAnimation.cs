using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public bool isRunning;
    [SerializeField]
    private Animator handAnimator;
    private PlayerMove playerMove;
    private float lerpedRun;
    private void Start() {
        playerMove = GetComponent<PlayerMove>();
    }
    private void Update() {
        lerpedRun = Mathf.Lerp(lerpedRun, ((isRunning&&playerMove.isGrounded)?1f:0f), Time.deltaTime * 10f);
        handAnimator.SetFloat("WalkSpeed", lerpedRun);
    }
    
}
