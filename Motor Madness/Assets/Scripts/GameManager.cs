using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameData gameData;
    public int currentCharacterIndex;
    public static GameManager Instance;
    

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

        SceneManager.LoadScene("Stage1.2");
    }

    public void SetRacerNames()
    {
        for (int i = 0; i < gameData.characters.Count; i++)
        {
            gameData.characters[i].SetRacerNameForOwnedCars();
        }
    }

}
