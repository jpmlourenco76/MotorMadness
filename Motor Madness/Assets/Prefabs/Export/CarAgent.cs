using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CarAgent : Agent
{

    public CheckpointManager _checkpointManager;
    public CarController2 _carController;
    private bool isOnWall = false;
    private float wallTimer = 0.0f;
    private const float wallDurationThreshold = 0.5f;
    private string id;
    public bool hitwall = false;

    public float Timer = 0.0f;
    

    public float throttleInput { get; private set; }
    public float steeringInput { get; private set; }

    public float BrakeReverse { get; private set; }

    public bool handBrakeInput { get; private set; }


    public override void Initialize()
    {
       
        _carController = GetComponent<CarController2>();

        
    }
 

    public override void OnEpisodeBegin()
    {
        id = _carController.carID;
        _checkpointManager.ResetCheckpoints();
        _carController.Respawn();
        _carController.isEngineRunning = true;
        isOnWall = false;
        hitwall = false;
        wallTimer = 0.0f;
        Timer = 0.0f;
    }

    private void Update()
    {
        if(_carController.KPH < 5f)
        {
            Timer += Time.deltaTime;
            if(Timer > 6f)
            {
                EndEpisode();
            }
        }

        if(_carController.KPH < 40f)
        {
            AddReward(-0.009f);
        }
        if (_carController.KPH > 40f)
        {
            if(!hitwall)
            {
                AddReward(_carController.KPH / 25000); // Adjust the reward scale as needed
            }
            else
            {
                AddReward(_carController.KPH / 100000);
            }
            if (_carController.KPH > 120f)
            {
                AddReward(_carController.KPH / 250000);
            }

         }


        if (isOnWall)
        {
            AddReward(-0.05f);
           
            wallTimer += Time.deltaTime;
            if (wallTimer >= wallDurationThreshold)
            {
                isOnWall = false;
                EndEpisode();

            }
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = _checkpointManager.nextCheckPointToReach.transform.position - transform.position;
        sensor.AddObservation(diff / 25f);
        sensor.AddObservation(_carController.KPH);
        AddReward(-0.002f);

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
           hitwall= true;
            isOnWall = true;
            
           EndEpisode();


        }
       
    }
  
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isOnWall = false;
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var Actions = actions.ContinuousActions;
        var discreteActions = actions.DiscreteActions;
       
      
        if (Mathf.Abs(Actions[0]) < 0.1f) Actions[0] = 0f;

        if(Actions[1] > 0)
        {
            throttleInput = Actions[1];
            BrakeReverse = 0;
        }
        else if (Actions[1] <0)
        {
            BrakeReverse= Mathf.Abs(Actions[1]);
            throttleInput = 0;
        }
        
        steeringInput = Actions[0];

        if (discreteActions[0] == 1)
        {
            handBrakeInput = true;
        }
        else
        {
            handBrakeInput= false;
        }

        _carController.UpdateCar();
        _carController.UpdateEngine();
        _carController.UpdateTransmission();
        _carController.UpdateBrake();
        _carController.UpdateSteer();
        _carController.ApplyWheelPositions();

        /*
                _carController.SetInput(Actions[1], Actions[0], discreteActions[0]);
                _carController.addDownForce();
                _carController.ApplyWheelPositions();
                _carController.calculateEnginePower();
        */

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
       var continousActions = actionsOut.ContinuousActions;
        var discreteActions = actionsOut.DiscreteActions;

        discreteActions.Clear();
        continousActions.Clear();
       
        continousActions[0] = Input.GetAxis("Horizontal");
        continousActions[1] = Input.GetKey(KeyCode.W) ? 1f : 0f; 
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;

    }

   



}
