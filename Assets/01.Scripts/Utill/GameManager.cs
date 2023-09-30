using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject player;
    public GameObject Player {get{return player;} private set{player = value;}}
    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerPrefab = Resources.Load("Prefabs/Player").GameObject();
    }
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "FactoryScene")
        {
            Player = Instantiate(playerPrefab);
            Player.transform.position = new Vector3(0f,0f,0f);
        }
    }
    public void HiGameManager()
    {

    }
}
