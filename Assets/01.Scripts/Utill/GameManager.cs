using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject playerPrefab;
    private PlayerMove player;
    public PlayerMove Player {get{return player;} private set{player = value;}}
    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerPrefab = Resources.Load("Prefabs/Player").GameObject();
    }
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "FactoryScene")
        {
            Player = Instantiate(playerPrefab).GetComponent<PlayerMove>();
            Player.Controller.enabled = false;
            Player.transform.position = new Vector3(0f,5f,0f);
            Player.Controller.enabled = true;
        }
    }
    public void HiGameManager()
    {

    }
}
