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
    private PlayerMove playerMove;
    private float lerpedRun;
    private float changeBlend;
    [SerializeField]
    private float changeDeley = 0.2f;
    private Coroutine changeToolCorutine;
    private void Awake() {
        foreach (var tool in animatorOverideByEnums)
        {
            animatorDictionary.Add(tool.tool, tool.animatorOverrideController);
        }
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
        yield return new WaitForSeconds(0.05f);
        handAnimator.runtimeAnimatorController = animatorDictionary[tool];
        changeBlend = 0f;
        handAnimator.SetFloat("ChangeBlend", changeBlend);
        handAnimator.SetTrigger("ChangeTool");
    }
    private void Start() {
        playerMove = GetComponent<PlayerMove>();
    }
    private void Update() {
        if(changeBlend <= 1)
        {
            changeBlend += Time.deltaTime / changeDeley;
            handAnimator.SetFloat("ChangeBlend", changeBlend);
        }
        lerpedRun = Mathf.Lerp(lerpedRun, ((isRunning&&playerMove.isGrounded)?1f:0f), Time.deltaTime * 10f);
        handAnimator.SetFloat("WalkSpeed", lerpedRun);
    }
    
}
