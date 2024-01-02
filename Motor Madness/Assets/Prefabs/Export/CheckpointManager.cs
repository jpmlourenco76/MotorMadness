using UnityEngine;
using System;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> Checkpoints;
    public float MaxTimeToReachNextCheckpoint = 60f;
    public float TimeLeft;
    public float totaltime;
    public float EndTime = 5f;
    public bool finished = false;
    public int CurrentCheckpointIndex = 0;
    public Checkpoint lastCheckpoint;
    public Checkpoint nextCheckPointToReach;


    public event Action<Checkpoint> reachedCheckpoint;

    public CarAgent carAgent;

    void Start()
    {
        
        Checkpoints = FindObjectOfType<Checkpoints>().checkPoints;
        ResetCheckpoints();
    }

    public void ResetCheckpoints()
    {
        CurrentCheckpointIndex = 0;
        TimeLeft = MaxTimeToReachNextCheckpoint;
        totaltime = 0;
        SetNextCheckpoint();
    }

    private void Update()
    {
        Vector3 carToCheckpoint = nextCheckPointToReach.transform.position - transform.position;
        carToCheckpoint.Normalize();
        float directionDot = Vector3.Dot(transform.forward, carToCheckpoint);
       
        if (directionDot > 0f)
        {
            carAgent.AddReward(0.00005f);

        }

        TimeLeft -= Time.deltaTime;
        if (TimeLeft < 0f)
        {
            // Time has run out
            carAgent.AddReward(-1f);
            carAgent.EndEpisode();
        }
    }

    public void CheckPointReached(Checkpoint checkpoint)
    {
        if (nextCheckPointToReach != checkpoint) return;
        
        lastCheckpoint = Checkpoints[CurrentCheckpointIndex];
       // reachedCheckpoint?.Invoke(checkpoint);
        CurrentCheckpointIndex++;
        
        if (CurrentCheckpointIndex >= Checkpoints.Count)
        {
            // All checkpoints reached, give a final reward



            float timeReward = Mathf.Clamp((8*(totaltime / (MaxTimeToReachNextCheckpoint * Checkpoints.Count))), 0f, 8f);

            timeReward = timeReward - 3.5f;

            if (carAgent.hitwall)
            {
                carAgent.AddReward(-0.2f);
                Debug.Log("w" + totaltime + " " + MaxTimeToReachNextCheckpoint + " " + timeReward);
                if (timeReward< 0)
                {
                    carAgent.AddReward(timeReward * 1.5f);
                }
                else
                {
                    carAgent.AddReward(timeReward * 0.5f);
                }
                 ;

            }
            else
            {

                Debug.Log("nw" +timeReward);
                carAgent.AddReward(timeReward);

            }
            carAgent.hitwall = false;
            
            carAgent.EndEpisode();
            return;
        }
        else 
        {
            // Reward for passing a checkpoint
            totaltime +=  TimeLeft;
            
            if (!carAgent.hitwall)
            {
                carAgent.AddReward(carAgent._carController.KPH / (30 * Checkpoints.Count));
            }
            else
            {
                carAgent.AddReward(carAgent._carController.KPH / (100 * Checkpoints.Count));
            }
            
            
        }

        SetNextCheckpoint();
    }

    private void SetNextCheckpoint()
    {
        if (CurrentCheckpointIndex < Checkpoints.Count)
        {
            

            TimeLeft = MaxTimeToReachNextCheckpoint;
            nextCheckPointToReach = Checkpoints[CurrentCheckpointIndex];
        }
    }
}
