using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    private bool timerRunning = false;
    private float startTime;
    public PlayerCarInput carinput;


    private void Awake()
    {
        carinput = GetComponent<PlayerCarInput>();
    }
    void Update()
    {
        if (carinput.throttleInput > 0 && !timerRunning)
        {
            Debug.Log("Started");
            StartTimer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Timer"))
        {
            DisplayElapsedTime();
        }
    }

    void StartTimer()
    {
        
        timerRunning = true;
        startTime = Time.time;
       
    }

    void DisplayElapsedTime()
    {
        if (timerRunning)
        {
            float elapsedTime = Time.time - startTime;
            Debug.Log($"Elapsed time: {elapsedTime:F2} seconds.");
        }
        
    }
}
