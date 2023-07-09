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
    public ToolEmum curToolEnum = ToolEmum.Empty;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(curToolEnum == ToolEmum.Scanner)
            {
                curToolEnum = ToolEmum.Empty;
            }
            else
            {
                curToolEnum = ToolEmum.Scanner;
            }
        }
    }
}
