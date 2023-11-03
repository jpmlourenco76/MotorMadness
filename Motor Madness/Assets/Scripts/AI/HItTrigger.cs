using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HItTrigger : MonoBehaviour
{
    private CarStateMAchine carStateMachine;

    public float detectionRange = 10.0f;
    private bool timerRunning = false;
    private float timer = 0.0f;

    private void Awake()
    {
        carStateMachine = GetComponent<CarStateMAchine>();
    }


    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Car") )
            {

                if (carStateMachine != null && carStateMachine.hit != true)
                {
                    carStateMachine.SetState(CarStateMAchine.CarState.Hit);
                    timerRunning = true;
                }
              
            }
            
        }

        if (timerRunning )
        {
            timer += Time.deltaTime;
            if(timer > 10.0f) {
                timerRunning = false;
                timer = 0.0f;  
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
