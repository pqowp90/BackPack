using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMove playerMove;
    public PlayerTool playerTool;
    private void Awake() {
        playerMove = GetComponent<PlayerMove>();
        playerTool = GetComponent<PlayerTool>();
    }
}
