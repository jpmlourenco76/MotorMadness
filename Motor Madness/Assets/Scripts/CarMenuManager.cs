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
    private float timerDuration = 3f;
    private float fadeInDuration = 1f;
    private float timer;

    public GameObject originalCanvas;
    public GameObject canvasHolder;

    private bool goLevel = false;
    private bool panelOne = false;
    private bool panelTwo = false;
    private bool panelThree = false;
    private bool isNormalLevel = false;
    private bool isTimerActive = false;

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
        specialPanelOne.alpha = 0f;
        specialPanelTwo.alpha = 0f;
        specialPanelThree.alpha = 0f;

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
            isTimerActive = false;
            timer = 0f;

            if (gameManager.gameData.characters[0].currentLevel == 2)
            {
                canvasHolder.SetActive(true);
                panelOne = true;
            }
            else if (gameManager.gameData.characters[0].currentLevel == 4)
            {
                canvasHolder.SetActive(true);
                panelTwo = true;
            }
            else if (gameManager.gameData.characters[0].currentLevel == 6)
            {
                canvasHolder.SetActive(true);
                panelThree = true;
            }
            else
            {
                gameManager.GoLevel(vehiclePointer);
            }
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

#region Special Level Panels

    void Update()
    {
        if (isTimerActive)
        {
            timer += Time.deltaTime;
            
            if(timer >= timerDuration)
            {
                gameManager.GoLevel(vehiclePointer);
            }
        }

        if (panelOne)
        {
            timer += Time.deltaTime;
            specialPanelOne.alpha = Mathf.Clamp01(timer / fadeInDuration);

            if(timer >= fadeInDuration)
            {
                panelOne = false;
                timer = 0f;
                isTimerActive = true;
            }
        }

        if (panelTwo)
        {
            timer += Time.deltaTime;
            specialPanelTwo.alpha = Mathf.Clamp01(timer / fadeInDuration);

            if(timer >= fadeInDuration)
            {
                panelTwo = false;
                timer = 0f;
                isTimerActive = true;
            }
        }

        if (panelThree)
        {
            timer += Time.deltaTime;
            specialPanelThree.alpha = Mathf.Clamp01(timer / fadeInDuration);

            if(timer >= fadeInDuration)
            {
                panelThree = false;
                timer = 0f;
                isTimerActive = true;
            }
        }
    }
#endregion
}
