using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStateMAchine : MonoBehaviour
{
    public enum CarState
    {
        Stopped,
        Follow,
        Hit
        
    }
    private CarController2 aICarController;
    private AIInput aIInput;
    private float wanderamount;
    private bool timerRunning = false;
    private float timer = 0.0f;
    public bool followpath;
    public float timepath;
    public LevelManager levelManager;
    private bool once = true;
    public GameObject minimapMarker;
    private bool isMMInstanciated;

    [HideInInspector] public bool hit;


    public CarState currentState = CarState.Stopped;


    private void Awake()
    {
        GameObject gameLogic = GameObject.Find("GameLogic");

        levelManager = gameLogic.GetComponent<LevelManager>();

        aICarController = GetComponent<CarController2>();
        aIInput = GetComponent<AIInput>();
        wanderamount = aIInput.wanderAmount;
        aICarController.StartEngineDellay = 0;
        once = true;
    }


    

    private void Update()
    {
        if(once && levelManager.startLevel)
        {
            aICarController.persuitTarget = GameObject.Find("Car0").gameObject;
            once = false;
        }


        switch (currentState)
        {
            case CarState.Stopped:
                aICarController.isEngineRunning = false;
                hit = false;
                if (isMMInstanciated)
                {
                    Destroy(minimapMarker);
                    isMMInstanciated = false;
                }
                break;

            case CarState.Follow:
                aICarController.isEngineRunning=true;
                if (!isMMInstanciated)
                {
                    Instantiate(minimapMarker);
                    isMMInstanciated=true;
                }


                if (timerRunning) {
                    timer += Time.deltaTime;
                    if (followpath)
                    {
                        if (timer >  timepath)
                        {
                            aICarController.persuitAiOn = true;
                            aICarController.persuitDistance = 2;
                        }
                    } 
                      else
                        {
                            aICarController.persuitAiOn = true;
                            aICarController.persuitDistance = 2;
                        }

                }

               
                hit = false;
                break;

            case CarState.Hit:


                if (timerRunning)
                {
                   
                    if (followpath)
                    {
                        if (timer > timepath)
                        {
                            aICarController.persuitAiOn = true;
                            aICarController.persuitDistance = 0;

                        }
                    }
                    else
                    {
                        aICarController.persuitAiOn = true;
                        aICarController.persuitDistance = 0;
                    }

                }
                aIInput.wanderAmount = wanderamount*1.3f;
               
                hit = true;
                break;
        }
    }

    public void SetState(CarState newState)
    {
        currentState = newState;
    }
}
