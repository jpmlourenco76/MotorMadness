using UnityEngine;
using System;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> Checkpoints;
    public float MaxTimeToReachNextCheckpoint = 60f;
    public float TimeLeft;
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
        SetNextCheckpoint();
    }

    private void Update()
    {
        if (CurrentCheckpointIndex >= Checkpoints.Count)
        {
            // All checkpoints reached, give a final reward

            if (carAgent.hitwall)
            {
                carAgent.AddReward(-2.0f);
                Debug.Log("CheckWall");
            }
            else
            {
                carAgent.AddReward(1.0f);
                Debug.Log("Check 1");
            }
            carAgent.hitwall = false;
            //Debug.Log("All checkpoints reached.");
            carAgent.EndEpisode();
            return;
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

        if (CurrentCheckpointIndex < Checkpoints.Count)
        {
            // Reward for passing a checkpoint
            Vector3 carToCheckpoint = nextCheckPointToReach.transform.position - transform.position;
            carToCheckpoint.Normalize();
            float directionDot = Vector3.Dot(transform.forward, carToCheckpoint);
            
            if (directionDot > 0f)
            {
                carAgent.AddReward(0.001f); 
               
            }
            float timeReward = (MaxTimeToReachNextCheckpoint - TimeLeft) / MaxTimeToReachNextCheckpoint;
            timeReward = Mathf.Clamp(timeReward, 0f, 20f); 
           if(carAgent.hitwall)
            {
                carAgent.AddReward(timeReward / ( 2 * Checkpoints.Count));

                carAgent.AddReward((20f) / (2 * Checkpoints.Count));
            }
            else
            {
                carAgent.AddReward(timeReward / Checkpoints.Count);
                carAgent.AddReward((20f) / Checkpoints.Count);
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
