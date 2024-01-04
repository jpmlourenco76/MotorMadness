using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    public bool hasCutscene;
    public bool isRaining;
    public bool isNight;

    public List<videocontroller> videocontroller;
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
    private GameObject th6PlaceDisplay;
    private GameObject th7PlaceDisplay;
    private GameObject th8PlaceDisplay;
    private GameObject th9PlaceDisplay;
    private GameObject th10PlaceDisplay;

    private GameObject stPlaceDisplayR;
    private GameObject ndPlaceDisplayR;
    private GameObject rdPlaceDisplayR;
    private GameObject thPlaceDisplayR;
    private GameObject fthPlaceDisplayR;
    private GameObject th6PlaceDisplayR;
    private GameObject th7PlaceDisplayR;
    private GameObject th8PlaceDisplayR;
    private GameObject th9PlaceDisplayR;
    private GameObject th10PlaceDisplayR;


    private GameObject stPointsR;
    private GameObject ndPointsR;
    private GameObject rdPointsR;
    private GameObject thPointsR;
    private GameObject fthPointsR;
    private GameObject th6PointsR;
    private GameObject th7PointsR;
    private GameObject th8PointsR;
    private GameObject th9PointsR;
    private GameObject th10PointsR;

    private SpawnPointManager spawnPointManager;
    public Transform[] spawnPoints;
    private bool special = false;

    private GameManager gameManager;

    public int levelReward = 0;
    public bool startLevel = false;

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
       
        if (hasCutscene)
        {
            videocontroller = new List<videocontroller>(GameObject.Find("CutSceneManager").gameObject.GetComponentsInChildren<videocontroller>());

        }


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
        th6PlaceDisplay = GameObject.Find("6thPlaceDisplay");
        th7PlaceDisplay = GameObject.Find("7thPlaceDisplay");
        th8PlaceDisplay = GameObject.Find("8hPlaceDisplay");
        th9PlaceDisplay = GameObject.Find("9thPlaceDisplay");
        th10PlaceDisplay = GameObject.Find("10thPlaceDisplay");


        stPlaceDisplayR = GameObject.Find("1stPlaceDisplayR");
        ndPlaceDisplayR = GameObject.Find("2ndPlaceDisplayR");
        rdPlaceDisplayR = GameObject.Find("3rdPlaceDisplayR");
        thPlaceDisplayR = GameObject.Find("4thPlaceDisplayR");
        fthPlaceDisplayR = GameObject.Find("5thPlaceDisplayR");
        th6PlaceDisplayR = GameObject.Find("6thPlaceDisplayR");
        th7PlaceDisplayR = GameObject.Find("7thPlaceDisplayR");
        th8PlaceDisplayR = GameObject.Find("8hPlaceDisplayR");
        th9PlaceDisplayR = GameObject.Find("9thPlaceDisplayR");
        th10PlaceDisplayR = GameObject.Find("10thPlaceDisplayR");

        stPointsR = GameObject.Find("1stPoints");
        ndPointsR = GameObject.Find("2ndPoints");
        rdPointsR = GameObject.Find("3rdPoints");
        thPointsR = GameObject.Find("4thPoints");
        fthPointsR = GameObject.Find("5thPoints");
        th6PointsR = GameObject.Find("6thPoints");
        th7PointsR = GameObject.Find("7thPoints");
        th8PointsR = GameObject.Find("8thPoints");
        th9PointsR = GameObject.Find("9thPoints");
        th10PointsR = GameObject.Find("10thPoints");



    }
    bool ended;
    private void Start()
    {
        ended = true;
        RaceRank.enabled = false;
        OverallRank.enabled = false;
        RetryCanvas.enabled = false;
        Canvas.enabled = false;
        MiniMap.enabled = false;

        gameManager.inrace = true;

        if(characterData.currentLevel == 3 )
        {
            StartCoroutine(PlayVideoAndSpawn());
        }
        else if (characterData.currentLevel == 5)
        {
            StartCoroutine(PlayVideoAndSpawn());
        }
        else if (characterData.currentLevel == 6)
        {
            StartCoroutine(PlayVideoAndSpawn());
        }
        else
        {
            Spawn();
        }

        
    }

    IEnumerator PlayVideoAndSpawn()
    {
        // Assuming videocontroller is an instance of VideoController
        yield return StartCoroutine(videocontroller[0].PlayVideoSingle());

        while (!videocontroller[0].videoCompleted)
        {
            yield return null;
        }

        Spawn();
    }

    public void Spawn()
    {

        
        AiControllers.Clear();
        cars.Clear();
        startLevel = true;
        Canvas.enabled = true;
        MiniMap.enabled = true;




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
                if (gameManager.levelType == GameManager.LevelType.Story)
                {
                    Car.gameObject.GetComponentInChildren<MeshRenderer>().material = gameManager.gameData.characters[i].SelectedCar.material;

                }
                Car.gameObject.GetComponent<CarController2>().StartEngineInAwake = true;
                if (isNight)
                {
                   
                    Car.gameObject.GetComponent<CarLights>().LightsOnSpawn = true;
                
                }

                if (isRaining)
                {
                    Car.gameObject.GetComponentInChildren<CarVFX>().raining = true;
                }


                cars.Add(gameManager.gameData.characters[i].SelectedCar);
                cars[i].lap = 0;
                cars[i].distance = 0;
                cars[i].wanderAmount = Car.gameObject.GetComponent<AIInput>().wanderAmount;

                if (i == 0)
                {
                    carController = Car.GetComponent<CarController2>();
                    carController.SetDriverType(CarController2.driver.Human);
                    gameManager.SetPlayerCarController(Car);
                    ApplyTorqueUpgrade(Car);
                    ApplyGearUpgrade(Car);
                    ApplyBreakUpgrade(Car);
                    ApplyTireUpgrade(Car);
                   // MiniMap.GetComponent<MiniMapController>().target = Car.gameObject.transform;

                }
                else
                {
                   if(AiControllers != null)
                    {
                        AiControllers.Add(Car.GetComponent<CarController2>());
                      //  Car.gameObject.GetComponent<Cameras>().enabled = false;
                      //  Car.gameObject.transform.GetChild(9).gameObject.SetActive(false);
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
                    if (gameManager.gameData.characters[i].currentLevel == 3 && gameManager.levelType == GameManager.LevelType.Story) {
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

                    if (gameManager.levelType == GameManager.LevelType.Story)
                    {
                        Car.gameObject.GetComponentInChildren<MeshRenderer>().material = gameManager.gameData.characters[i].SelectedCar.material;

                    }
                    Car.gameObject.GetComponent<CarController2>().StartEngineInAwake = true;
                    if (isNight)
                    {
                        Car.gameObject.GetComponent<CarLights>().MainLightsIsOn = true;
                        Car.gameObject.GetComponent<CarLights>().SwitchMainLights();
                    }

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
                    //MiniMap.GetComponent<MiniMapController>().target = Car.gameObject.transform;


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


    private void FixedUpdate()
    {
        AIInput ai;
        for (int i = 1; i < cars.Count; i++)
        {
            ai = GameObject.Find("Car" + i).GetComponent<AIInput>();
            if (ai!= null)
            {
                if (!special)
                {
                    if (cars[i].checkpoints - GameObject.Find("Car0").GetComponent<CarController2>().pchecks > 2)
                    {
                        GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount = GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount * 0.9999f;
                        if (GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount < 0.15f) GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount = 0.15f;
                    }
                    else
                    {
                        GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount = cars[i].wanderAmount;
                    }
                }
                else
                {
                    if (cars[i].checkpoints - GameObject.Find("Car0").GetComponent<CarController2>().pchecks > 2)
                    {
                        GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount = GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount * 0.9999f;
                        if (GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount < 0.15f) GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount = 0.15f;
                    }
                    else
                    {
                        GameObject.Find("Car" + i).GetComponent<AIInput>().wanderAmount = 0.8f;
                    }
                }
                
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
                th6PlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[5].RacerName;
                th7PlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[6].RacerName;
                th8PlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[7].RacerName;
                th9PlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[8].RacerName;
                th10PlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[9].RacerName;

            }


            RaceRank.enabled = true;
        }
        Invoke("EnableOverallRank", 3);
    }

    private void EnableOverallRank()
    {
        if (gameManager.levelType == GameManager.LevelType.Story)
        {

            int[] prize = { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1};

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
                                characterData.money += (int)(levelReward * 0.2f);
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
                                characterData.money += (int)(levelReward * 0.85f);
                            }
                            else if (i == 2)
                            {
                                characterData.money += (int)(levelReward * 0.75f);
                            }
                            else if (i == 3)
                            {
                                characterData.money += (int)(levelReward * 0.68f);
                            }
                            else if (i == 4)
                            {
                                characterData.money += (int)(levelReward * 0.5f);
                            }
                            else if (i == 5)
                            {
                                characterData.money += (int)(levelReward * 0.4f);
                            }
                            else if (i == 6)
                            {
                                characterData.money += (int)(levelReward * 0.3f);
                            }
                            else if (i == 7)
                            {
                                characterData.money += (int)(levelReward * 0.25f);
                            }
                            else if (i == 8)
                            {
                                characterData.money += (int)(levelReward * 0.2f);
                            }
                            else if (i == 9)
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
            th6PlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[5].RacerName;
            th7PlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[6].RacerName;
            th8PlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[7].RacerName;
            th9PlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[8].RacerName;
            th10PlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[9].RacerName;
               



            stPointsR.GetComponent<TextMeshProUGUI>().text = Rank[0].points.ToString();
            ndPointsR.GetComponent<TextMeshProUGUI>().text = Rank[1].points.ToString();
            rdPointsR.GetComponent<TextMeshProUGUI>().text = Rank[2].points.ToString();
            thPointsR.GetComponent<TextMeshProUGUI>().text = Rank[3].points.ToString();
            fthPointsR.GetComponent<TextMeshProUGUI>().text = Rank[4].points.ToString();
            th6PointsR.GetComponent<TextMeshProUGUI>().text = Rank[5].points.ToString();
            th7PointsR.GetComponent<TextMeshProUGUI>().text = Rank[6].points.ToString();
            th8PointsR.GetComponent<TextMeshProUGUI>().text = Rank[7].points.ToString();
            th9PointsR.GetComponent<TextMeshProUGUI>().text = Rank[8].points.ToString();
            th10PointsR.GetComponent<TextMeshProUGUI>().text = Rank[9].points.ToString();



                OverallRank.enabled = true;



        }


        for (int i = 1; i < gameManager.gameData.characters.Count; i++)
        {
            gameManager.gameData.characters[i].SelectedCar.CarID = 0;
        }


        if (gameManager.gameData.characters[0].currentLevel == 3)
        {
            gameManager.gameData.characters[0].OwnedCars.Add(gameManager.gameData.GameCars[2]);

                if(gameManager.levelType == GameManager.LevelType.Story)
                {
                    gameManager.SetRacerNames();
                    gameManager.SetPointsPerRacer();
                    gameManager.updateMaterials();
                }
            
        }

        if(gameManager.gameData.characters[0].currentLevel == 10)
            {
                int winner;
                if (Rank[0].RacerName == gameManager.gameData.characters[0].characterName)
                {
                    winner = 0;
                }
                else
                {
                    winner = 1;

                }
                OverallRank.enabled = false;
                StartCoroutine(PlayVideoAndGoGarage(winner));
            }
            else
            {
                Invoke("GoGarage", 3);
            }


      

        }
        else
        {

            RaceRank.enabled = false;
            RetryCanvas.enabled = true;
           
        }

    }

    IEnumerator PlayVideoAndGoGarage(int i)
    {
        
        if(i == 0)
        {
            yield return StartCoroutine(videocontroller[i].PlayVideo());
        }
        else if(i == 1) {

            yield return StartCoroutine(videocontroller[i].PlayVideoSingle());
        }

      

        while (!videocontroller[i].videoCompleted)
        {
            yield return null;
        }

        GoMenu();
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
                newTorque = OriginalTorque * 1.15f;
                break;
            case 2:
                newTorque = OriginalTorque * 1.3f;
                break;
            case 3:
                newTorque = OriginalTorque * 1.6f;
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
                newGear = OriginalGear * 1.2f;
                break;
            case 2:
                newGear = OriginalGear * 1.35f;
                break;
            case 3:
                newGear = OriginalGear * 1.5f;
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
                    newBreak = OriginalBreak * 1.3f;
                    break;
                case 2:
                    newBreak = OriginalBreak * 1.85f;
                    break;
                case 3:
                    newBreak = OriginalBreak * 2.70f;
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
        float OriginalTire = car.gameObject.GetComponent<CarController2>().wheels[0].stiffnessupgrade;
        float newTire;

        float rainValue = 0;

        if (isRaining)
        {
            rainValue = -0.4f;

        }

        for (int i = 0; i < car.gameObject.GetComponent<CarController2>().wheels.Length; i++)
        {

            switch (gameManager.gameData.characters[0].SelectedCar.upgradelvls.TireUpdrage)
            {
                case 0:
                    newTire = OriginalTire + rainValue;
                    break;
                case 1:
                    newTire = 0.15f + rainValue;
                    break;
                case 2:
                    newTire = 0.25f + rainValue;
                    break;
                case 3:
                    newTire = 0.45f + rainValue;
                    break;
                default:
                    newTire = OriginalTire + rainValue;
                    break;

            }
            car.gameObject.GetComponent<CarController2>().wheels[i].stiffnessupgrade = newTire;


        }
    }


}
