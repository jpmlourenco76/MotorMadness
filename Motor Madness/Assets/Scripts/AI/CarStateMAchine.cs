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

    [HideInInspector] public bool hit;


    public CarState currentState = CarState.Stopped;


    private void Awake()
    {
        aICarController = GetComponent<CarController2>();
        aIInput = GetComponent<AIInput>();
        wanderamount = aIInput.wanderAmount;
        aICarController.StartEngineDellay = 0;
    }


    

    private void Update()
    {
        switch (currentState)
        {
            case CarState.Stopped:
                aICarController.isEngineRunning = false;
                hit = false;
                break;

            case CarState.Follow:
                aICarController.isEngineRunning=true;
                timerRunning = true;

                if(timerRunning) {
                    timer += Time.deltaTime;
                    if (timer > 5f)
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
                    timer += Time.deltaTime;
                    if (timer > 5f)
                    {
                        aICarController.persuitAiOn = true;
                        aICarController.persuitDistance = 2;

                    }

                }
                aIInput.wanderAmount = wanderamount*1.3f;
                aICarController.persuitDistance = 0;
                hit = true;
                break;
        }
    }

    public void SetState(CarState newState)
    {
        currentState = newState;
    }
}
