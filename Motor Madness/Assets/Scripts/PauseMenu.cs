using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static bool gameIsPaused = false;
    public GameObject pauseMenu;
    private GameManager gameManager;
    private GameData gameData;

    private void Awake()
    {
        gameManager = GameManager.Instance;

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
                gameManager.inrace = false;
                Pause();
            }
        }        
    }

    public void Resume()
    {
        
        pauseMenu.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
        gameManager.inrace = true;
        
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0f;
        
    }

    public void Quit()
    {
        if(gameManager.levelType == GameManager.LevelType.Story)
        {
            Resume();
            SceneManager.LoadScene("Garage");
        }
        else
        {
            Resume();
            SceneManager.LoadScene("MainMenu");

        }
        
    }
}
