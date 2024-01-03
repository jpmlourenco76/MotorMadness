using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameData gameData;
    public int currentCharacterIndex;
    public static GameManager Instance;
    public CarController2 playerCarController;
    public int  desiredLevel;
    public int currentcar;
    public bool inrace = false;
    public enum LevelType
    {
        Story,
        Training,
        QuickPlay,
    }
    [SerializeField] public LevelType levelType;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

      // SetRacerNames();
    }

    public CharacterData GetCurrentCharacter()
    {
        return gameData.characters[currentCharacterIndex];
    }

    public void GoGarage()
    {
        SceneManager.LoadScene("Garage");
    }
    public void GoGarageTraining()
    {
        SceneManager.LoadScene("GarageTraining");
    }

    public void GoShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void GoMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    

    public void GoLevel(int carPointer)
    {
        currentcar = carPointer;
        if(levelType == LevelType.QuickPlay || levelType == LevelType.Training)
        {
            gameData.characters[currentCharacterIndex].SelectedCar = gameData.GameCars[carPointer];
            if (desiredLevel == 1)
            {
                SceneManager.LoadScene("Stage1.1");
            }
            else if (desiredLevel == 2)
            {
                SceneManager.LoadScene("Stage1.2");
            }
            else if (desiredLevel == 3)
            {
                SceneManager.LoadScene("Stage1.3");
            }
            else if (desiredLevel == 4)
            {
                SceneManager.LoadScene("Stage2.1");
            }
            else if (desiredLevel == 5)
            {
                SceneManager.LoadScene("Stage2.2");
            }
            else if (desiredLevel == 6)
            {
                SceneManager.LoadScene("Stage2.3");
            }
            else if (desiredLevel == 7)
            {
                SceneManager.LoadScene("Stage3.1");
            }
            else if (desiredLevel == 8)
            {
                SceneManager.LoadScene("Stage3.2");
            }
            else if (desiredLevel == 9)
            {
                SceneManager.LoadScene("Stage3.3");
            }
            else if (desiredLevel == 10)
            {
                SceneManager.LoadScene("Stage4");
            }
            else
            {
                Debug.Log("WIN");
            }
        }
        else
        {
            gameData.characters[currentCharacterIndex].SelectedCar = gameData.characters[currentCharacterIndex].OwnedCars[carPointer];
            if (gameData.characters[0].currentLevel == 1)
            {
                SceneManager.LoadScene("Stage1.1");
            }
            else if (gameData.characters[0].currentLevel == 2)
            {
                SceneManager.LoadScene("Stage1.2");
            }
            else if (gameData.characters[0].currentLevel == 3)
            {
                SceneManager.LoadScene("Stage1.3");
            }
            else if (gameData.characters[0].currentLevel == 4)
            {
                SceneManager.LoadScene("Stage2.1");
            }
            else if (gameData.characters[0].currentLevel == 5)
            {
                SceneManager.LoadScene("Stage2.2");
            }
            else if (gameData.characters[0].currentLevel == 6)
            {
                SceneManager.LoadScene("Stage2.3");
            }
            else if (gameData.characters[0].currentLevel == 7)
            {
                SceneManager.LoadScene("Stage3.1");
            }
            else if (gameData.characters[0].currentLevel == 8)
            {
                SceneManager.LoadScene("Stage3.2");
            }
            else if (gameData.characters[0].currentLevel == 9)
            {
                SceneManager.LoadScene("Stage3.3");
            }
            else if (gameData.characters[0].currentLevel == 10)
            {
                SceneManager.LoadScene("Stage4");
            }
            else
            {
                Debug.Log("WIN");
            }
        }

       
    }



    public void SetRacerNames()
    {
        for (int i = 0; i < gameData.characters.Count; i++)
        {
            gameData.characters[i].SetRacerNameForOwnedCars();
        }
    }

    public void SetPlayerCarController(GameObject car)
    {
        playerCarController = car.GetComponent<CarController2>();
    }

    public void SetLevelType(LevelType newLevel)
    {
        levelType = newLevel;
    }

    public void SetPointsPerRacer()
    {
        for (int i = 0; i < gameData.characters.Count; i++)
        {
            gameData.characters[i].setPoints();
        }
    }
    public void updateMaterials()
    {
        for(int i = 0;i < gameData.characters.Count;i++)
        {
            gameData.characters[i].updateMaterials();
        }
    }
}
