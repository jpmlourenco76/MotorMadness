using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       

        CheckpointManager checkpointManager = other.GetComponent<CheckpointManager>();
       if(checkpointManager != null)
        {
            checkpointManager.CheckPointReached(this);
        }
            
        
        
    }
}

