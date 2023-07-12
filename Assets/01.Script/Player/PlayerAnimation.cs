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
    private Animator LegAnimator;
    [SerializeField]
    private Animator ShadowAnimagtor;
    private PlayerMove playerMove;
    private float lerpedRun;
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
        
        LegAnimator.SetLayerWeight(1, 0f);
        ShadowAnimagtor.SetLayerWeight(1, 1f);
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
        ShadowAnimagtor.SetTrigger("Changing");
        yield return new WaitForSeconds(changeDeley);
        handAnimator.runtimeAnimatorController = animatorDictionary[tool];
        ShadowAnimagtor.runtimeAnimatorController = animatorDictionary[tool];
        changeBlend = 0f;
        ShadowAnimagtor.SetLayerWeight(1, 1f);
        changeBlend = 0f;
        handAnimator.SetFloat("ChangeBlend", changeBlend);
        ShadowAnimagtor.SetFloat("ChangeBlend", changeBlend);
        handAnimator.SetTrigger("ChangeTool");
        ShadowAnimagtor.SetTrigger("ChangeTool");
    }
    private void Start() {
        playerMove = GetComponent<PlayerMove>();
    }
    public void Jump()
    {
        ShadowAnimagtor.SetTrigger("Jump");
        handAnimator.SetTrigger("Jump");
        LegAnimator.SetTrigger("Jump");
    }
    private void Update() {
        if(changeBlend <= 1)
        {
            changeBlend += Time.deltaTime / changeDeley;
            handAnimator.SetFloat("ChangeBlend", changeBlend);
            ShadowAnimagtor.SetFloat("ChangeBlend", changeBlend);
        }
        lerpedRun = Mathf.Lerp(lerpedRun, ((isRunning&&playerMove.isGrounded)?1f:0f), Time.deltaTime * 10f);
        handAnimator.SetFloat("WalkSpeed", lerpedRun);
        ShadowAnimagtor.SetFloat("WalkSpeed", lerpedRun);
        
        lerpedMoveDir = Vector3.Lerp(lerpedMoveDir, moveDir, Time.deltaTime * 10f);

        LegAnimator.SetFloat("X", lerpedMoveDir.x);
        LegAnimator.SetFloat("Y", lerpedMoveDir.z);
        ShadowAnimagtor.SetFloat("X", lerpedMoveDir.x);
        ShadowAnimagtor.SetFloat("Y", lerpedMoveDir.z);

        ShadowAnimagtor.SetBool("IsGround", playerMove.isGrounded);
        LegAnimator.SetBool("IsGround", playerMove.isGrounded);
        handAnimator.SetBool("IsGround", playerMove.isGrounded);
        
    }
    
}
