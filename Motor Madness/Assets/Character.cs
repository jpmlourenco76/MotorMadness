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
    

    

    private void Start()
    {
        gameManager = GameManager.Instance;
        mainMenu = GameObject.Find("Main Camera").gameObject.GetComponent<MainMenu>();
         
    }

    public void OnButtonClick()
    {

        oldMaterials = gameManager.gameData.characters[0].CarMaterials;
        gameManager.gameData.characters[0].characterName = gameManager.gameData.characters[id].characterName;
        gameManager.gameData.characters[0].CarMaterials = gameManager.gameData.characters[id].CarMaterials;

        gameManager.gameData.characters[id].characterName = "Apex";
        gameManager.gameData.characters[id].CarMaterials = oldMaterials;
        

        gameManager.gameData.characters[0].position = 10;

        for(int i = 1;  i < 10; i++)
        {
            if(gameManager.gameData.characters[0].position == gameManager.gameData.characters[i].position)
            {
                gameManager.gameData.characters[i].position = 1;
            }
        }

        gameManager.updateMaterials();
        mainMenu.GoGarage();

    }
}
