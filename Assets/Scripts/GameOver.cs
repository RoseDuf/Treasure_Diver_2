using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    //GAME OVER BUTTON HANDLER

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene(string name)
    {
        if (LevelManager.modePlayed == "Game" && name != "MainMenu")
        {
            name = LevelManager.modePlayed;
        }

        Debug.Log("Scene Load:" + name);
        //Application.LoadLevel(name);
        SceneManager.LoadScene(name);
    }
}
