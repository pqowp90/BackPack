using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimation : MonoBehaviour
{
    [Serializable]
    public class AnimatorOverideByEnum
    {
        public ToolEmum tool;
        public AnimatorOverrideController animatorOverrideController;
    }
    private Dictionary<ToolEmum, AnimatorOverrideController> animatorDictionary = new Dictionary<ToolEmum, AnimatorOverrideController>();
    [SerializeField]
    private List<AnimatorOverideByEnum> animatorOverideByEnums = new List<AnimatorOverideByEnum>();
    public bool isRunning;
    [SerializeField]
    private Animator handAnimator;
    [SerializeField]
    private Animator legAnimator;
    [SerializeField]
    private Animator shadowAnimagtor;
    private PlayerMove playerMove;
    private float lerpedRun;
    private float run;
    private float changeBlend;
    [SerializeField]
    private float changeDeley = 0.2f;
    private Coroutine changeToolCorutine;
    public Vector3 moveDir;
    private Vector3 lerpedMoveDir;
    private void Awake() {
        foreach (var tool in animatorOverideByEnums)
        {
            animatorDictionary.Add(tool.tool, tool.animatorOverrideController);
        }
        if(legAnimator)
            handAnimator.SetLayerWeight(1, 0f);
        if(shadowAnimagtor)
            shadowAnimagtor.SetLayerWeight(1, 1f);
    }
    public void ChangeToolAnimation(ToolEmum tool)
    {
        if(changeToolCorutine != null)
        {
            StopCoroutine(changeToolCorutine);
            changeToolCorutine = null;
        }
        changeToolCorutine = StartCoroutine(ChangeToolState(tool));
    }
    private IEnumerator ChangeToolState(ToolEmum tool)
    {
        handAnimator.SetTrigger("Changing");
        shadowAnimagtor.SetTrigger("Changing");
        yield return new WaitForSeconds(changeDeley);
        handAnimator.runtimeAnimatorController = animatorDictionary[tool];
        shadowAnimagtor.runtimeAnimatorController = animatorDictionary[tool];
        changeBlend = 0f;
        shadowAnimagtor.SetLayerWeight(1, 1f);
        changeBlend = 0f;
        handAnimator.SetFloat("ChangeBlend", changeBlend);
        shadowAnimagtor.SetFloat("ChangeBlend", changeBlend);
        handAnimator.SetTrigger("ChangeTool");
        shadowAnimagtor.SetTrigger("ChangeTool");
    }
    private void Start() {
        playerMove = GetComponent<PlayerMove>();
    }
    public void Jump()
    {
        shadowAnimagtor.SetTrigger("Jump");
        handAnimator.SetTrigger("Jump");
        legAnimator.SetTrigger("Jump");
    }
    private void Update() {
        if(changeBlend <= 1)
        {
            changeBlend += Time.deltaTime / changeDeley;
            handAnimator.SetFloat("ChangeBlend", changeBlend);
            shadowAnimagtor.SetFloat("ChangeBlend", changeBlend);
        }
        run = ((isRunning&&playerMove.isGrounded)?1f:0f) + moveDir.magnitude;
        lerpedRun = Mathf.Lerp(lerpedRun, run, Time.deltaTime * 10f);
        handAnimator.SetFloat("WalkSpeed", lerpedRun);
        legAnimator.SetFloat("WalkSpeed", lerpedRun);
        shadowAnimagtor.SetFloat("WalkSpeed", lerpedRun);
        
        lerpedMoveDir = Vector3.Lerp(lerpedMoveDir, moveDir * ((isRunning&&playerMove.isGrounded)?2f:1f), Time.deltaTime * 10f);

        legAnimator.SetFloat("X", lerpedMoveDir.x);
        legAnimator.SetFloat("Y", lerpedMoveDir.z);
        shadowAnimagtor.SetFloat("X", lerpedMoveDir.x);
        shadowAnimagtor.SetFloat("Y", lerpedMoveDir.z);

        shadowAnimagtor.SetBool("IsGround", playerMove.isGrounded);
        legAnimator.SetBool("IsGround", playerMove.isGrounded);
        handAnimator.SetBool("IsGround", playerMove.isGrounded);
        
    }
    
}
