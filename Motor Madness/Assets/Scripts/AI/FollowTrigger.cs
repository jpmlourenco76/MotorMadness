using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrigger : MonoBehaviour
{
    public CarStateMAchine carStateMachine;

   

    private void OnTriggerEnter(Collider other)
    {
        
            if (carStateMachine != null)
            {
                carStateMachine.SetState(CarStateMAchine.CarState.Follow);
            }
        
    }

   
}
