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

       SetRacerNames();
    }

    public CharacterData GetCurrentCharacter()
    {
        return gameData.characters[currentCharacterIndex];
    }

    public void GoGarage()
    {
        SceneManager.LoadScene("Garage");
    }

    public void GoShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void GoLevel(int carPointer)
    {

        gameData.characters[currentCharacterIndex].SelectedCar = gameData.characters[currentCharacterIndex].OwnedCars[carPointer];

        if (gameData.characters[0].currentLevel == 1)
        {
            SceneManager.LoadScene("Stage1.2");
        }
        else if (gameData.characters[0].currentLevel == 2)
        {
            SceneManager.LoadScene("Stage1.3");
        }
        else if (gameData.characters[0].currentLevel == 3)
        {
            SceneManager.LoadScene("Stage2.1");
        }
        else if (gameData.characters[0].currentLevel == 4)
        {
            SceneManager.LoadScene("Stage2.3");
        }
        else if (gameData.characters[0].currentLevel == 5)
        {
            SceneManager.LoadScene("Stage3.1");
        }
        else if (gameData.characters[0].currentLevel == 6)
        {
            SceneManager.LoadScene("Stage3.3");
        }
        else if (gameData.characters[0].currentLevel == 7)
        {
            SceneManager.LoadScene("Stage4");
        }
        else
        {
            Debug.Log("WIN");
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

    public void SetPointsPerRacer()
    {
        for (int i = 0; i < gameData.characters.Count; i++)
        {
            gameData.characters[i].setPoints();
        }
    }
}
