using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolEmum
{
    Empty,
    Scanner,

}

public class PlayerTool : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    public ToolEmum curToolEnum = ToolEmum.Empty;
    private void Awake() {
        playerAnimation = GetComponent<PlayerAnimation>();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(curToolEnum == ToolEmum.Scanner)
            {
                curToolEnum = ToolEmum.Empty;
                playerAnimation.ChangeToolAnimation(curToolEnum);
            }
            else
            {
                curToolEnum = ToolEmum.Scanner;
                playerAnimation.ChangeToolAnimation(curToolEnum);
            }
        }
    }
}
