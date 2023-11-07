using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Laps3Completed : MonoBehaviour
{

    public GameObject CurrentMinDisplay;
    public GameObject CurrentSecDisplay;
    public GameObject CurrentMilDisplay;

    public GameObject BestMinDisplay;
    public GameObject BestSecDisplay;
    public GameObject BestMilDisplay;

    public GameObject LapCounter;
    public int LapsDone;

    public CarController2 carController;

    private LapTimeManager lapTimeManager;
    private bool lapCompleted = false;

    public AudioSource Finish;

    void Start()
    {
        lapTimeManager = GetComponent<LapTimeManager>();
    }

    void OnTriggerEnter()
    {

        // Check if the required number of laps (e.g., 2 laps) has been completed
        if (carController.laps >= 3)
        {
            // End the race and display the finish panel
            EndRace();
        }
    }
    private void OnTriggerExit(Collider other)
    {


        LapCounter.GetComponent<TextMeshProUGUI>().text = "" + carController.laps;
        if (carController.laps > 0  && carController.human)
        {
            lapTimeManager.UpdateBestLapTime();

            // Display the current lap time
            CurrentMinDisplay.GetComponent<TextMeshProUGUI>().text = lapTimeManager.MinuteCount.ToString("D2") + ".";
            CurrentSecDisplay.GetComponent<TextMeshProUGUI>().text = lapTimeManager.SecondCount.ToString("D2") + ".";
            CurrentMilDisplay.GetComponent<TextMeshProUGUI>().text = lapTimeManager.MilliCount.ToString("F0");
            // Display the best lap time
            BestMinDisplay.GetComponent<TextMeshProUGUI>().text = lapTimeManager.BestMinDisplay.ToString("D2") + ".";
            BestSecDisplay.GetComponent<TextMeshProUGUI>().text = lapTimeManager.BestSecDisplay.ToString("D2") + ".";
            BestMilDisplay.GetComponent<TextMeshProUGUI>().text = lapTimeManager.BestMiliDisplay.ToString("F0");



            // Reset lap timer
            lapTimeManager.ResetLapTime();
            carController.human =false;
        }

        
    }



    void EndRace()
    {
        // Enable the Finish UI panel
        Finish.Play();
    }
}