using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int id;
    private GameManager gameManager;
    private List<Material> oldMaterials;
    private MainMenu mainMenu;
    private bool videoInProgress = false;
    private string oldname;

    public List<videocontroller> videos;


    private void Start()
    {
        gameManager = GameManager.Instance;
        mainMenu = GameObject.Find("Main Camera").gameObject.GetComponent<MainMenu>();
        videos = new List<videocontroller>(GameObject.Find("CutSceneManager").gameObject.GetComponentsInChildren<videocontroller>());

    }

    public void OnButtonClick()
    {

  

        if(!videoInProgress)
        {
            StartCoroutine(PlayVideoAndGoGarage(videos[1]));
        }

       
    }


    IEnumerator PlayVideoAndGoGarage(videocontroller video)
    {

        oldMaterials = gameManager.gameData.characters[0].CarMaterials;
         oldname = gameManager.gameData.characters[0].characterName;
        

        gameManager.gameData.characters[0].characterName = gameManager.gameData.characters[id].characterName;

        videoInProgress = true;
        // Assuming videocontroller is an instance of VideoController
        yield return StartCoroutine(video.PlayVideo());

       

        while (!video.videoCompleted)
        {
            yield return null;
        }

        videoInProgress = false;
        HandleGoGarage();


    }


    private void HandleGoGarage()
    {

        
      
        
        gameManager.gameData.characters[0].characterName = gameManager.gameData.characters[id].characterName;
        gameManager.gameData.characters[0].CarMaterials = gameManager.gameData.characters[id].CarMaterials;

        gameManager.gameData.characters[id].characterName = oldname;
        gameManager.gameData.characters[id].CarMaterials = oldMaterials;

        gameManager.gameData.characters[0].OwnedCars.Clear();



        if (gameManager.gameData.characters[0].characterName.Contains("Apex"))
        {
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[0]);
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[12]);

        }
        else if (gameManager.gameData.characters[0].characterName.Contains("Nitro"))
        {
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[1]);
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[3]);
        }
        else if (gameManager.gameData.characters[0].characterName.Contains("Blaze"))
        {
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[0]);
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[9]);
        }
        else if (gameManager.gameData.characters[0].characterName.Contains("Shadow"))
        {
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[4]);
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[11]);
        }
        else if (gameManager.gameData.characters[0].characterName.Contains("Viper"))
        {
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[1]);
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[8]);
        }




        gameManager.gameData.characters[0].position = 10;

        for (int i = 1; i < 10; i++)
        {
            if (gameManager.gameData.characters[0].position == gameManager.gameData.characters[i].position)
            {
                gameManager.gameData.characters[i].position = 1;
            }
        }

       gameManager.updateMaterials();



        if (gameManager != null)
        {
            gameManager.GoGarage();
        }
        else
        {
            mainMenu.GoGarage();
        }
    }
}
