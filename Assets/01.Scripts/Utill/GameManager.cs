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
    private Player player;
    public Player Player {get
        {
            if(player is null)
            {
                return CreatePlayer();
            }
            return player;
        }
        private set{player = value;}
    }
    private Player CreatePlayer(Vector3 pos = default)
    {
        if(!playerPrefab)
            playerPrefab = Resources.Load("Prefabs/Player").GameObject();
        player = Instantiate(playerPrefab).GetComponent<Player>();
        player.playerMove.Controller.enabled = false;
        player.transform.position = pos;
        player.playerMove.Controller.enabled = true;
        return player;
    }
    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "FactoryScene")
        {
            player = CreatePlayer(new Vector3(0f, 5f, 0f));
        }
    }
    public void HiGameManager()
    {

    }
}
