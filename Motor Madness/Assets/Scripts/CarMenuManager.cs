using Den.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CarMenuManager : MonoBehaviour
{
    public bool hasCutscene;
    public CharacterData characterData;
    public GameObject toRotate;
    public float rotateSpeed;

    public int vehiclePointer = 0;
    private GameManager gameManager;
    public bool isShop;
    private GameObject Price;
    public List<CarData> ShopCars;
    private Vector3 position = new Vector3(0.5f, 0.5f, 0.75f);


    #region Variables for Panels
    private float timerDuration = 1f;
    private float fadeInDuration = 0.5f;
    private float timer;

    public GameObject originalCanvas;
    public GameObject canvasHolder;

    private bool goLevel = false;
    private bool panelOne = false;
    private bool panelTwo = false;
    private bool panelThree = false;
    private bool isNormalLevel = false;
    private bool isTimerActive = false;

    public List<videocontroller> videos;

    public CanvasGroup specialPanelOne;
    public CanvasGroup specialPanelTwo;
    public CanvasGroup specialPanelThree;
    #endregion


    private void Awake()
    {
        
        

        Price = GameObject.Find("PriceLabel");
        if (hasCutscene)
        {
            videos = new List<videocontroller>(GameObject.Find("CutSceneManager").gameObject.GetComponentsInChildren<videocontroller>());

        }

    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        characterData = gameManager.GetCurrentCharacter();
        ShopCars = (
            from gameCar in gameManager.gameData.GameCars
            join ownedCar in characterData.OwnedCars on gameCar.CarID equals ownedCar.CarID into ownedCarsGroup
            where !ownedCarsGroup.Any()
            select gameCar).ToList();

           
            gameManager.SetRacerNames();
            gameManager.SetPointsPerRacer();
        gameManager.updateMaterials();



        if (specialPanelOne != null)
        {
            
            if (gameManager.gameData.characters[0].currentLevel == 3 && gameManager.levelType == GameManager.LevelType.Story)
            {
                
                specialPanelOne.alpha = 1f;
                canvasHolder.SetActive(true);
                panelOne = true;
            }
            else
            {
                specialPanelOne.alpha = 0f;
            }
        }
        if (specialPanelTwo != null)
        {
            specialPanelTwo.alpha = 0f;
        }
        if (specialPanelThree != null)
        {
            specialPanelThree.alpha = 0f;
        }

        isTimerActive = false;
        timer = 0f;

       


        PlayerPrefs.SetInt("pointer", 0);

        vehiclePointer = PlayerPrefs.GetInt("pointer");


        if (gameManager.levelType == GameManager.LevelType.QuickPlay || gameManager.levelType == GameManager.LevelType.Training)
        {
            GameObject childObject = Instantiate(gameManager.gameData.GameCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;


        }
        else if (!isShop && gameManager.levelType == GameManager.LevelType.Story)
        {
            GameObject childObject = Instantiate(characterData.OwnedCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;
            childObject.GetComponentInChildren<MeshRenderer>().material = characterData.OwnedCars[vehiclePointer].material;
        }
        else
        {
            GameObject childObject = Instantiate(ShopCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;

        }

    }

    private void FixedUpdate()
    {
        if (isShop)
        {
            Price.GetComponent<TextMeshProUGUI>().text = "Price: " + ShopCars[vehiclePointer].price.ToString();

        }
        
        //toRotate.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void rightButton()
    {
        if ((vehiclePointer < characterData.OwnedCars.Count - 1 && !isShop && gameManager.levelType == GameManager.LevelType.Story) || (vehiclePointer < ShopCars.Count - 1 && isShop && gameManager.levelType == GameManager.LevelType.Story || vehiclePointer < gameManager.gameData.GameCars.Count - 1 && gameManager.levelType != GameManager.LevelType.Story))
        {
            Destroy(GameObject.FindGameObjectWithTag("Car"));
            vehiclePointer++;
            PlayerPrefs.SetInt("pointer", vehiclePointer);

            if (gameManager.levelType == GameManager.LevelType.QuickPlay || gameManager.levelType == GameManager.LevelType.Training)
            {
                GameObject childObject = Instantiate(gameManager.gameData.GameCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;

            }
            else if (!isShop && gameManager.levelType == GameManager.LevelType.Story)
            {
                GameObject childObject = Instantiate(characterData.OwnedCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;
                childObject.GetComponentInChildren<MeshRenderer>().material = characterData.OwnedCars[vehiclePointer].material;

            }
            else
            {
                GameObject childObject = Instantiate(ShopCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;

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

            if (gameManager.levelType == GameManager.LevelType.QuickPlay || gameManager.levelType == GameManager.LevelType.Training)
            {
                GameObject childObject = Instantiate(gameManager.gameData.GameCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;

            }
            else if (!isShop && gameManager.levelType == GameManager.LevelType.Story)
            {
                GameObject childObject = Instantiate(characterData.OwnedCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;
                childObject.GetComponentInChildren<MeshRenderer>().material = characterData.OwnedCars[vehiclePointer].material;

            }
            else
            {
                GameObject childObject = Instantiate(ShopCars[vehiclePointer].CarPrefab, position, Quaternion.identity) as GameObject;

            }
        }
    }


    public void OnBuy()
    {
        if (characterData.money > ShopCars[vehiclePointer].price)
        {
            characterData.money = characterData.money - ShopCars[vehiclePointer].price;

            characterData.OwnedCars.Add(ShopCars[vehiclePointer]);
            gameManager.SetRacerNames();
            gameManager.SetPointsPerRacer();
            ShopCars.Remove(ShopCars[vehiclePointer]);
            rightButton();
        }


    }


    public void OnSelect()
    {   
        if (gameManager != null)
        {
            
             if (gameManager.gameData.characters[0].currentLevel == 4 && gameManager.levelType == GameManager.LevelType.Story )
            {
                
                StartCoroutine(PlayVideoAndGoLevel(videos[0]));
            }
            else if (gameManager.gameData.characters[0].currentLevel == 7 && gameManager.levelType == GameManager.LevelType.Story )
            {
                
                StartCoroutine(PlayVideoAndGoLevel(videos[1]));
            }
            else if (gameManager.gameData.characters[0].currentLevel == 10 && gameManager.levelType == GameManager.LevelType.Story )
            {
                
                StartCoroutine(PlayVideoAndGoLevel(videos[2]));
            }
            else
            {
                if(gameManager.levelType == GameManager.LevelType.QuickPlay)
                {
                    gameManager.gameData.characters[0].position = 10;

                    for (int i = 1; i < 10; i++)
                    {
                        if (gameManager.gameData.characters[0].position == gameManager.gameData.characters[i].position)
                        {
                            gameManager.gameData.characters[i].position = 1;
                        }
                    }
                }
                gameManager.GoLevel(vehiclePointer);
            }
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }



    IEnumerator PlayVideoAndGoLevel(videocontroller video)
    {
        // Assuming videocontroller is an instance of VideoController
        yield return StartCoroutine(video.PlayVideo());

        while (!video.videoCompleted)
        {
            yield return null;
        }

        gameManager.GoLevel(vehiclePointer);

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
            

            if(timer >= fadeInDuration)
            {
                panelOne = false;
                timer = 0f;
                isTimerActive = true;
            }
        }

    
    }
#endregion
}
