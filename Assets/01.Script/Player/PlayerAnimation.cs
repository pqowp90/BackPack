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
    private void Awake() {
        foreach (var tool in animatorOverideByEnums)
        {
            animatorDictionary.Add(tool.tool, tool.animatorOverrideController);
        }
    }
    public void ChangeToolState(ToolEmum tool)
    {
        //handAnimator = animatorDictionary[tool];
    }
    private void Start() {
        playerMove = GetComponent<PlayerMove>();
    }
    private void Update() {
        lerpedRun = Mathf.Lerp(lerpedRun, ((isRunning&&playerMove.isGrounded)?1f:0f), Time.deltaTime * 10f);
        handAnimator.SetFloat("WalkSpeed", lerpedRun);
    }
    
}
