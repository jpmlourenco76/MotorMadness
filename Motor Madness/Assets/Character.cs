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

        oldMaterials = gameManager.gameData.characters[0].CarMaterials;
        gameManager.gameData.characters[0].characterName = gameManager.gameData.characters[id].characterName;
        gameManager.gameData.characters[0].CarMaterials = gameManager.gameData.characters[id].CarMaterials;

        gameManager.gameData.characters[id].characterName = "Apex";
        gameManager.gameData.characters[id].CarMaterials = oldMaterials;


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
