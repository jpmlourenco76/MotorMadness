using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapsCompleted : MonoBehaviour
{

    public GameObject CurrentMinDisplay;
    public GameObject CurrentSecDisplay;
    public GameObject CurrentMilDisplay;

    public GameObject BestMinDisplay;
    public GameObject BestSecDisplay;
    public GameObject BestMilDisplay;

    public GameObject LapCounter;
    public GameObject TotalLaps;
    private GameObject gameLogic;
    private LevelManager levelManager;
    

    public CarController2 carController;

    private LapTimeManager lapTimeManager;
    private GameManager gameManager;

    private bool once = true;

    public float totallaps;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        lapTimeManager = GetComponent<LapTimeManager>();
        gameLogic = GameObject.Find("GameLogic");
        levelManager = gameLogic.GetComponent<LevelManager>();

        CurrentMinDisplay = GameObject.Find("MinDisplay");
        CurrentSecDisplay = GameObject.Find("SecDisplay");
        CurrentMilDisplay = GameObject.Find("MilDisplay");
        BestMinDisplay = GameObject.Find("BestMinDisplay");
        BestSecDisplay = GameObject.Find("BestSecDisplay");
        BestMilDisplay = GameObject.Find("BestMilliDisplay");

        LapCounter = GameObject.Find("LapCount");
        TotalLaps = GameObject.Find("TotalLaps");
    }

    private void Start()
    {

        once = true;
    }

    private void Update()
    {
        if(once && levelManager.startLevel)
        {
            TotalLaps.GetComponent<TextMeshProUGUI>().text = "/ " + totallaps;
            carController = gameManager.playerCarController;
            once = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (carController.laps <= totallaps)
        {

            LapCounter.GetComponent<TextMeshProUGUI>().text = "" + carController.laps;
        }

        // Check if the required number of laps (e.g., 2 laps) has been completed
        if (carController.laps >= totallaps && carController.human)
        {

            levelManager.EndRace();
        }

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
            carController.human = false;
        }

        
    }



    
}