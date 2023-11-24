using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyButton : MonoBehaviour
{
    public void ShopBtn()
    {
        // Find the GameManager instance using the singleton pattern
        GameManager gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            // Call the function from GameManager
            gameManager.GoShop();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void GarageBtn()
    {
        // Find the GameManager instance using the singleton pattern
        GameManager gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            // Call the function from GameManager
            gameManager.GoGarage();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void MainMenuBtn()
    {
        // Find the GameManager instance using the singleton pattern
        GameManager gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            // Call the function from GameManager
            gameManager.GoMainMenu();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }


   
}
