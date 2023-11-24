using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Playables;

public class LevelManager : MonoBehaviour
{
    public int TotalLaps;

    public enum RaceType
    {
        Industrial,
        City,
        Countryside,
        RaceTrack,
        Special
    }
    [SerializeField] RaceType raceType;



    private GameObject FinishTrig;
    public CarController2 carController;
    private LapsCompleted lapsCompleted;
    private GameObject Finish;
    private AudioSource finish;
    private GameObject FinishPanel;
    private Canvas RaceRank;
    private Canvas OverallRank;
    private Canvas RetryCanvas;
    private Canvas MiniMap;
    private Canvas Canvas;
    public List<CarData> ShopCars;
    public CharacterData characterData;

    private GameObject stPlaceDisplay;
    private GameObject ndPlaceDisplay;
    private GameObject rdPlaceDisplay;
    private GameObject thPlaceDisplay;
    private GameObject fthPlaceDisplay;

    private GameObject stPlaceDisplayR;
    private GameObject ndPlaceDisplayR;
    private GameObject rdPlaceDisplayR;
    private GameObject thPlaceDisplayR;
    private GameObject fthPlaceDisplayR;

    private GameObject stPointsR;
    private GameObject ndPointsR;
    private GameObject rdPointsR;
    private GameObject thPointsR;
    private GameObject fthPointsR;

    private SpawnPointManager spawnPointManager;
    public Transform[] spawnPoints;
    private bool special = false;

    private GameManager gameManager;

    public int levelReward = 0;
    

    [SerializeField]
    public List<CarData> cars = new List<CarData>();

    [HideInInspector] public bool finished = false;
     public List<CarData> finishplacements = new List<CarData>();
    [HideInInspector] public List<CarData> Rank = new List<CarData>();
    public List<CarData> distanceArray;

    [HideInInspector]
    public List<CarController2> AiControllers;

    private void Awake()
    {

        gameManager = GameManager.Instance;
        characterData = gameManager.GetCurrentCharacter();

        FinishTrig = GameObject.Find("FinishTrigger");
        Finish = GameObject.Find("Finish");
        FinishPanel = GameObject.Find("FinishPanel");
        finish = Finish.GetComponent<AudioSource>();
        lapsCompleted = FinishTrig.GetComponent<LapsCompleted>();
        lapsCompleted.totallaps = TotalLaps;
        RaceRank = GameObject.Find("RaceRank").GetComponent<Canvas>();
        OverallRank = GameObject.Find("OverallRank").GetComponent<Canvas>();
        RetryCanvas = GameObject.Find("RetryCanvas").GetComponent<Canvas>();

        MiniMap = GameObject.Find("CanvasMiniMap").GetComponent<Canvas>();
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();


        spawnPointManager = GameObject.Find("SpawnPoints").GetComponent<SpawnPointManager>();
        spawnPoints = spawnPointManager.spawnPoints;


        stPlaceDisplay = GameObject.Find("1stPlaceDisplay");
        ndPlaceDisplay = GameObject.Find("2ndPlaceDisplay");
        rdPlaceDisplay = GameObject.Find("3rdPlaceDisplay");
        thPlaceDisplay = GameObject.Find("4thPlaceDisplay");
        fthPlaceDisplay = GameObject.Find("5thPlaceDisplay");

        stPlaceDisplayR = GameObject.Find("1stPlaceDisplayR");
        ndPlaceDisplayR = GameObject.Find("2ndPlaceDisplayR");
        rdPlaceDisplayR = GameObject.Find("3rdPlaceDisplayR");
        thPlaceDisplayR = GameObject.Find("4thPlaceDisplayR");
        fthPlaceDisplayR = GameObject.Find("5thPlaceDisplayR");

        stPointsR = GameObject.Find("1stPointsR");
        ndPointsR = GameObject.Find("2ndPointsR");
        rdPointsR = GameObject.Find("3rdPointsR");
        thPointsR = GameObject.Find("4thPointsR");
        fthPointsR = GameObject.Find("5thPointsR");



    }
    bool ended;
    private void Start()
    {
        ended = true;
        RaceRank.enabled = false;
        OverallRank.enabled = false;
        RetryCanvas.enabled = false;

        gameManager.inrace = true;
        Spawn();
    }

    public void Spawn()
    {
        
        AiControllers.Clear();
        cars.Clear();
        int i = 0;

        while (i < gameManager.gameData.characters.Count)
        {

            if (gameManager.gameData.characters[i].SelectedCar.CarID == 0)
            {
                int rnd = 0;
                switch (raceType)
                {
                    case RaceType.Industrial:       //stage1
                        rnd = Random.Range(0, 2);
                        break;
                    case RaceType.City:             //stage3
                        rnd = Random.Range(2, 4);
                        break;
                    case RaceType.Countryside:      //stage2
                        rnd = Random.Range(4, 6);
                        break;
                    case RaceType.RaceTrack:        //stage4
                        rnd = Random.Range(6, 8);
                        break;
                    case RaceType.Special:
                        
                        break;
                }

                if(!special) {
                    gameManager.gameData.characters[i].SelectedCar = gameManager.gameData.characters[i].OwnedCars[rnd];
                }

            }
            switch (raceType)
            {
                case RaceType.Special:
                    special = true;
                    break;

            }

            if (!special && gameManager.levelType != GameManager.LevelType.Training)
            {
                Vector3 pos = spawnPointManager.spawnPoints[gameManager.gameData.characters[i].position - 1].position;
                Quaternion rot = spawnPointManager.spawnPoints[gameManager.gameData.characters[i].position - 1].rotation;
                GameObject Car = Instantiate(gameManager.gameData.characters[i].SelectedCar.CarPrefab, pos, rot);
                Car.gameObject.name = "Car" + i;
                Car.gameObject.GetComponent<Rigidbody>().useGravity = true;
                Car.gameObject.GetComponent<CarController2>().race = true;
                Car.gameObject.GetComponent<AIInput>().enabled = true;
                Car.gameObject.GetComponentInChildren<MeshCollider>().enabled = true;
                Car.gameObject.GetComponent<CarAIWaipointTracker>().enabled = true;

                cars.Add(gameManager.gameData.characters[i].SelectedCar);
                cars[i].lap = 0;
                cars[i].distance = 0;

                if (i == 0)
                {
                    carController = Car.GetComponent<CarController2>();
                    carController.SetDriverType(CarController2.driver.Human);
                    gameManager.SetPlayerCarController(Car);
                    ApplyTorqueUpgrade(Car);
                    ApplyGearUpgrade(Car);
                    ApplyBreakUpgrade(Car);
                    ApplyTireUpgrade(Car);
                    MiniMap.GetComponent<MiniMapController>().target = Car.gameObject.transform;

                }
                else
                {
                   if(AiControllers != null)
                    {
                        AiControllers.Add(Car.GetComponent<CarController2>());
                        Car.gameObject.GetComponent<Cameras>().enabled = false;
                        Car.gameObject.transform.GetChild(9).gameObject.SetActive(false);
                    }
                    
                }
            }
            else if(special || gameManager.levelType == GameManager.LevelType.Training)
            {
                if( i== 0) {
                    
                    OverallRank = null;
                    Vector3 pos = spawnPointManager.spawnPoints[0].position;
                    Quaternion rot = spawnPointManager.spawnPoints[0].rotation;
                    GameObject Car;
                    if (gameManager.gameData.characters[i].currentLevel == 2 && gameManager.levelType == GameManager.LevelType.Story) {
                        RaceRank = null;
                        Car = Instantiate(gameManager.gameData.GameCars[2].CarPrefab, pos, rot);

                    }
                    else
                    {
                        Car = Instantiate(gameManager.gameData.characters[i].SelectedCar.CarPrefab, pos, rot);

                    }


                    Car.gameObject.name = "Car" + i;
                    Car.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    Car.gameObject.GetComponent<CarController2>().race = true;
                    Car.gameObject.GetComponentInChildren<MeshCollider>().enabled = true;
                    Car.gameObject.GetComponent<AIInput>().enabled = true;
                    Car.gameObject.GetComponent <CarAIWaipointTracker>().enabled = true;
                    cars.Add(gameManager.gameData.characters[i].SelectedCar);
                    cars[i].lap = 0;
                    cars[i].distance = 0;

                    carController = Car.GetComponent<CarController2>();
                    carController.SetDriverType(CarController2.driver.Human);
                    gameManager.SetPlayerCarController(Car);
                    ApplyTorqueUpgrade(Car);
                    ApplyGearUpgrade(Car);
                    ApplyBreakUpgrade(Car);
                    ApplyTireUpgrade(Car);
                    MiniMap.GetComponent<MiniMapController>().target = Car.gameObject.transform;


                }
            }

           
            i++;
        }


        if(gameManager.levelType == GameManager.LevelType.QuickPlay)
        {
            OverallRank = null;
            
        }
        else if(gameManager.levelType == GameManager.LevelType.Training)
        {
            RaceRank = null;
            OverallRank = null;
            TotalLaps = 50;
            lapsCompleted.totallaps = TotalLaps;
        }


       foreach(Transform checkpoint in spawnPointManager.spawnPoints) {
            if(checkpoint != null) {
                checkpoint.GetComponentInChildren<MeshRenderer>().material.color = Color.clear;
               
            }
            
        }
    }


    public void EndRace()
    {


        if (ended)
        {
            foreach (CarData car in distanceArray)
            {
                CarData carClone = (CarData)car.Clone();
                finishplacements.Add(carClone);
            }
            ended = false;
            carController.SetDriverType(CarController2.driver.AI);
        }




        FinishPanel.GetComponent<TextMeshProUGUI>().enabled = true;
        finish.Play();
        gameManager.inrace = false;





        Invoke("EnableRaceRank", 3);



    }


    private void EnableRaceRank()
    {     
        MiniMap.enabled = false;
        Canvas.enabled = false;

        if (RaceRank != null) 
        {
            if (special)
            {
                stPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[0].RacerName;
                ndPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[1].RacerName;
            }
            else
            {
                stPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[0].RacerName;
                ndPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[1].RacerName;
                rdPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[2].RacerName;
                thPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[3].RacerName;
                fthPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[4].RacerName;
            }


            RaceRank.enabled = true;
        }
        Invoke("EnableOverallRank", 3);
    }

    private void EnableOverallRank()
    {
        if (gameManager.levelType == GameManager.LevelType.Story)
        {

            int[] prize = { 25, 18, 15, 12, 10 };

        for (int i = 0; i < finishplacements.Count; i++)
        {
            foreach (CarData car in cars)
            {
                switch (raceType)
                {
                    case RaceType.Special:


                        break;
                    default:
                        if (car.RacerName == finishplacements[i].RacerName)
                        {
                            car.points += prize[i];

                        }
                        break;
                }
            }

            foreach (CharacterData characterData in gameManager.gameData.characters)
            {

                switch (raceType)
                {
                    case RaceType.Special:

                        if (characterData.characterName == finishplacements[i].RacerName)
                        {
                            if (i == 0)
                            {
                                characterData.money += levelReward;
                            }
                            else if (i == 1)
                            {
                                characterData.money += (int)(levelReward * 0.1f);
                            }
                        }
                        break;
                    default:

                        if (characterData.characterName == finishplacements[i].RacerName)
                        {
                            characterData.position = i + 1;

                            if (i == 0)
                            {
                                characterData.money += levelReward;
                            }
                            else if (i == 1)
                            {
                                characterData.money += (int)(levelReward * 0.7f);
                            }
                            else if (i == 2)
                            {
                                characterData.money += (int)(levelReward * 0.5f);
                            }
                            else if (i == 3)
                            {
                                characterData.money += (int)(levelReward * 0.3f);
                            }
                            else if (i == 4)
                            {
                                characterData.money += (int)(levelReward * 0.1f);
                            }
                        }
                        break;
                }

            }


        }

        gameManager.SetPointsPerRacer();

        
        if (OverallRank != null)
        {
            RaceRank.enabled = false;
            foreach (CarData car in cars)
            {

                CarData carClone = (CarData)car.Clone();
                Rank.Add(carClone);
                Rank = Rank.OrderByDescending(carClone => carClone.points).ToList();

            }




            stPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[0].RacerName;
            ndPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[1].RacerName;
            rdPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[2].RacerName;
            thPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[3].RacerName;
            fthPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[4].RacerName;

            stPointsR.GetComponent<TextMeshProUGUI>().text = Rank[0].points.ToString();
            ndPointsR.GetComponent<TextMeshProUGUI>().text = Rank[1].points.ToString();
            rdPointsR.GetComponent<TextMeshProUGUI>().text = Rank[2].points.ToString();
            thPointsR.GetComponent<TextMeshProUGUI>().text = Rank[3].points.ToString();
            fthPointsR.GetComponent<TextMeshProUGUI>().text = Rank[4].points.ToString();


            OverallRank.enabled = true;



        }


        for (int i = 1; i < gameManager.gameData.characters.Count; i++)
        {
            gameManager.gameData.characters[i].SelectedCar.CarID = 0;
        }


        if (gameManager.gameData.characters[0].currentLevel == 2)
        {
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[2]);
            gameManager.SetRacerNames();
            gameManager.SetPointsPerRacer();
        }

        
        Invoke("GoGarage", 3);

        }
        else
        {

            RaceRank.enabled = false;
            RetryCanvas.enabled = true;
           
        }

    }

    public void Restart()
    {
        gameManager.GoLevel(gameManager.currentcar);
    }

    public void GoMenu()
    {
        gameManager.GoMainMenu();
    }

    private void GoGarage()
    {
        gameManager.gameData.characters[0].currentLevel++;
        gameManager.GoGarage();
    }
    


    private void ApplyTorqueUpgrade(GameObject car)
    {
        float OriginalTorque = car.gameObject.GetComponent<CarController2>().Engine.MaxMotorTorque;
        float newTorque;

        switch (gameManager.gameData.characters[0].SelectedCar.upgradelvls.TorqueUpgrade)
        {
            case 0:
                newTorque = OriginalTorque;
                break;
            case 1:
                newTorque = OriginalTorque * 1.05f;
                break;
            case 2:
                newTorque = OriginalTorque * 1.2f;
                break;
            case 3:
                newTorque = OriginalTorque * 1.5f;
                break;
            default:
                newTorque = OriginalTorque;
                break;

        }
        car.gameObject.GetComponent<CarController2>().Engine.MaxMotorTorque = newTorque;
    }
    private void ApplyGearUpgrade(GameObject car)
    {
        float OriginalGear = car.gameObject.GetComponent<CarController2>().Gearbox.MainRatio;
        float newGear;

        switch (gameManager.gameData.characters[0].SelectedCar.upgradelvls.GearUpgrade)
        {
            case 0:
                newGear = OriginalGear;
                break;
            case 1:
                newGear = OriginalGear * 1.1f;
                break;
            case 2:
                newGear = OriginalGear * 1.2f;
                break;
            case 3:
                newGear = OriginalGear * 1.3f;
                break;
            default:
                newGear = OriginalGear;
                break;

        }
        car.gameObject.GetComponent<CarController2>().Gearbox.MainRatio = newGear;
    }

    private void ApplyBreakUpgrade(GameObject car)
    {
        float OriginalBreak = car.gameObject.GetComponent<CarController2>().wheels[0].MaxBrakeTorque;
        float newBreak;

        for (int i = 0; i < car.gameObject.GetComponent<CarController2>().wheels.Length; i++)
        {

            switch (gameManager.gameData.characters[0].SelectedCar.upgradelvls.BreakUpgrade)
            {
                case 0:
                    newBreak = OriginalBreak;
                    break;
                case 1:
                    newBreak = OriginalBreak * 1.25f;
                    break;
                case 2:
                    newBreak = OriginalBreak * 1.75f;
                    break;
                case 3:
                    newBreak = OriginalBreak * 2.50f;
                    break;
                default:
                    newBreak = OriginalBreak;
                    break;

            }
            car.gameObject.GetComponent<CarController2>().wheels[i].MaxBrakeTorque = newBreak;


        }


    }


    private void ApplyTireUpgrade(GameObject car)
    {
        float OriginalTire = car.gameObject.GetComponent<CarController2>().wheels[0].GroundStiffness;
        float newTire;

        for (int i = 0; i < car.gameObject.GetComponent<CarController2>().wheels.Length; i++)
        {

            switch (gameManager.gameData.characters[0].SelectedCar.upgradelvls.TireUpdrage)
            {
                case 0:
                    newTire = OriginalTire;
                    break;
                case 1:
                    newTire = OriginalTire * 1.1f;
                    break;
                case 2:
                    newTire = OriginalTire * 1.2f;
                    break;
                case 3:
                    newTire = OriginalTire * 1.35f;
                    break;
                default:
                    newTire = OriginalTire;
                    break;

            }
            car.gameObject.GetComponent<CarController2>().wheels[i].GroundStiffness = newTire;


        }
    }


}
