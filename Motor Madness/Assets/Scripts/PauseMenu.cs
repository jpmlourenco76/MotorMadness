using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private static bool gameIsPaused = false;
    public GameObject pauseMenu;
    private GameManager gameManager;
    private GameData gameData;

    void Start()
    {
        gameIsPaused = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }        
    }

    private void Resume()
    {
        pauseMenu.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        gameIsPaused = true;
        //Time.timeScale = 0f;
    }

    private void Quit()
    {
        Invoke("GoGarage", gameManager.gameData.characters[0].currentLevel);
        Time.timeScale = 1f;
    }
}
