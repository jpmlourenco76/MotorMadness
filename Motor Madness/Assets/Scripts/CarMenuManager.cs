using Den.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CarMenuManager : MonoBehaviour
{
    public CharacterData characterData;
    public GameObject toRotate;
    public float rotateSpeed;

    public int vehiclePointer = 0;
    private GameManager gameManager;
    public bool isShop;
    private GameObject Price;
    public List<CarData> ShopCars;

    #region Variables for Panels
    private bool threeSecondsDone = false;
    private float timerDuration = 3f;
    private float timer;

    public GameObject originalCanvas;
    public GameObject canvasHolder;

    private bool goLevel = false;
    private bool specialPanel = false;

    public CanvasGroup specialPanelOne;
    public CanvasGroup specialPanelTwo;
    public CanvasGroup specialPanelThree;
    #endregion


    private void Awake()
    {
        gameManager = GameManager.Instance;
        characterData = gameManager.GetCurrentCharacter();
        ShopCars = (
            from gameCar in gameManager.gameData.GameCars
            join ownedCar in characterData.OwnedCars on gameCar.CarID equals ownedCar.CarID into ownedCarsGroup
            where !ownedCarsGroup.Any()
            select gameCar).ToList();

        Price = GameObject.Find("PriceLabel");
    }
    private void Start()
    {
        PlayerPrefs.SetInt("pointer", 0);
        vehiclePointer = PlayerPrefs.GetInt("pointer");

        if (!isShop)
        {
            GameObject childObject = Instantiate(characterData.OwnedCars[vehiclePointer].CarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            childObject.transform.parent = toRotate.transform;
        }
        else
        {
            GameObject childObject = Instantiate(ShopCars[vehiclePointer].CarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            childObject.transform.parent = toRotate.transform;

        }

    }

    private void FixedUpdate()
    {
        if (isShop)
        {
            Price.GetComponent<TextMeshProUGUI>().text = "Price: " + ShopCars[vehiclePointer].price.ToString();

        }
        toRotate.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void rightButton()
    {
        if ((vehiclePointer < characterData.OwnedCars.Count - 1 && !isShop) || (vehiclePointer < ShopCars.Count - 1 && isShop))
        {
            Destroy(GameObject.FindGameObjectWithTag("Car"));
            vehiclePointer++;
            PlayerPrefs.SetInt("pointer", vehiclePointer);

            if (!isShop)
            {
                GameObject childObject = Instantiate(characterData.OwnedCars[vehiclePointer].CarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                childObject.transform.parent = toRotate.transform;
            }
            else
            {
                GameObject childObject = Instantiate(ShopCars[vehiclePointer].CarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                childObject.transform.parent = toRotate.transform;
            }
        }
    }

    public void leftButton()
    {
        if (vehiclePointer > 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("Car"));
            vehiclePointer--;
            PlayerPrefs.SetInt("pointer", vehiclePointer);

            if (!isShop)
            {
                GameObject childObject = Instantiate(characterData.OwnedCars[vehiclePointer].CarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                childObject.transform.parent = toRotate.transform;
            }
            else
            {
                GameObject childObject = Instantiate(ShopCars[vehiclePointer].CarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                childObject.transform.parent = toRotate.transform;
            }
        }
    }


    public void OnBuy()
    {
        if (characterData.money > ShopCars[vehiclePointer].price)
        {
            characterData.money = characterData.money - ShopCars[vehiclePointer].price;

            characterData.OwnedCars.Add(ShopCars[vehiclePointer]);
            ShopCars.Remove(ShopCars[vehiclePointer]);
            rightButton();
        }


    }


    public void OnSelect()
    {
        // Find the GameManager instance using the singleton pattern
        GameManager gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            // Call the function from GameManager
            threeSecondsDone = false;
            originalCanvas.SetActive(false);
            canvasHolder.SetActive(true);
            timer = 0;
            specialPanel = true;

            //gameManager.GoLevel(vehiclePointer);
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

#region Special Level Panels

    private void Update()
    {
        if (specialPanel)
        {
            if (gameManager.gameData.characters[0].currentLevel == 2)
            {
                if (specialPanelOne.alpha < 1)
                {
                    specialPanelOne.alpha += Time.deltaTime;
                }

                if (specialPanelOne.alpha >= 1 && timer <= timerDuration)
                {
                    timer += Time.deltaTime;
                }

                if (specialPanelOne.alpha >= 1 && timer >= timerDuration)
                {
                    goLevel = true;
                }

            }

            if (gameManager.gameData.characters[0].currentLevel == 4)
            {
                if (specialPanelTwo.alpha < 1)
                {
                    specialPanelTwo.alpha += Time.deltaTime;
                }

                if (specialPanelTwo.alpha >= 1 && timer <= timerDuration)
                {
                    timer += Time.deltaTime;
                }

                if (specialPanelTwo.alpha >= 1 && timer >= timerDuration)
                {
                    goLevel = true;
                }

            }

            if (gameManager.gameData.characters[0].currentLevel == 6)
            {
                if (specialPanelThree.alpha < 1)
                {
                    specialPanelThree.alpha += Time.deltaTime;
                }

                if (specialPanelThree.alpha >= 1 && timer <= timerDuration)
                {
                    timer += Time.deltaTime;                    
                }

                if(specialPanelThree.alpha >= 1 && timer >= timerDuration)
                {
                    goLevel = true;
                }
            }

            if (goLevel)
            {
                gameManager.GoLevel(vehiclePointer);
            }
        }
        
    }

#endregion
}
