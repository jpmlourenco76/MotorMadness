using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCar : MonoBehaviour
{

    private GameManager gameManager;
    private LevelManager levelManager;
    public GameObject car;
    public CarController2 controller2;
    public bool stage3 = false;
    private bool once = true;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        controller2 = GetComponent<CarController2>();
        levelManager = GameObject.Find("GameLogic").GetComponent<LevelManager>();
    
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        if(once && levelManager.startLevel)
        {
            AddNewCar();
            once = false;
        }
    }

    private void AddNewCar()
    {
        CarData newCar = new CarData
        {
            CarID = 23,
            CarName = "1v1",
            RacerName = "Challenger",
            CarPrefab = car, 
            distance = 0f,
            lap = 0f,
            points = 0,
            price = 0,
            upgradelvls = new CarUpgrade()
        };


        levelManager.cars.Add(newCar);
        levelManager.AiControllers.Add(GetComponent<CarController2>());
        if (!stage3)
        {
            
            gameObject.GetComponent<Cameras>().enabled = false;
            gameObject.transform.GetChild(9).gameObject.SetActive(false);
        }
      
    }
}
