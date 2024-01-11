using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private static bool gameIsPaused = false;
    public GameObject pauseMenu;
    private GameManager gameManager;
    private GameData gameData;
    private bool toPause;
    private bool canceledAction;    

    private void Awake()
    {
        gameManager = GameManager.Instance;

    }

    private void OnEnable()
    {      
        var pauseAction = new CarInput().Menu.OpenClosePause;
        pauseAction.performed += PauseAction;
        pauseAction.canceled += NoPauseAction;
        pauseAction.Enable();
    }

    private void PauseAction(InputAction.CallbackContext value)
    {
        if(!toPause && !gameIsPaused)
        {
            gameManager.inrace = false;
            Pause();
            toPause = true;
        }
        else if(!toPause && gameIsPaused)
        {
            Resume();
            toPause = true;
        }
    }

    public void NoPauseAction(InputAction.CallbackContext value)
    {
        toPause=false;
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
            return;
        }
        else
        {
            Resume();
            SceneManager.LoadScene("MainMenu");
            return;
        }
        
    }
}
