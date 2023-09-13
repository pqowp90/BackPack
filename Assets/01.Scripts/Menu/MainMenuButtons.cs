using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.LoadScene("FactoryScene");
    }
}
