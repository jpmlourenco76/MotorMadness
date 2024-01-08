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
    public bool timerRunning = false;
    private float timer = 0.0f;
    public bool followpath;
    public float timepath;
    public LevelManager levelManager;
    private bool once = true;
    public GameObject minimapMarker;
    public GameObject minimapInstance;
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
        isMMInstanciated = false;
    }


    

    private void Update()
    {
        if(once && levelManager.startLevel)
        {
            aICarController.persuitTarget = GameObject.Find("Car0").gameObject;
            once = false;
           
        }
        if (isMMInstanciated)
        {
            UpdatePrefabPosition();
        }

        switch (currentState)
        {



            case CarState.Stopped:
                aICarController.isEngineRunning = false;
                hit = false;
            /*   if (isMMInstanciated)
                {
                    Destroy(minimapMarker);
                    isMMInstanciated = false;
                }*/
                break;

            case CarState.Follow:
                aICarController.StartEngineInAwake = true;
                aICarController.isEngineRunning=true;
                
                if (!isMMInstanciated)
                {
                  minimapInstance =  Instantiate(minimapMarker);
                    // Set the initial position based on the current GameObject's position
                    minimapInstance.transform.position = new Vector3(
                        transform.position.x,
                        transform.position.y + 140,
                        transform.position.z
                    );

                    // Set the initial rotation to (90, 0, 0)
                    minimapInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
                    isMMInstanciated =true;
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



    private void UpdatePrefabPosition()
    {
        // Update the position based on the current GameObject's position
        if (minimapInstance != null)
        {
            minimapInstance.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 140,
                transform.position.z
            );
            minimapInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
