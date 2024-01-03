using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapTimeManager : MonoBehaviour
{
    [HideInInspector] public GameObject FinishTrig;

    [HideInInspector] public int MinuteCount;
    [HideInInspector] public int SecondCount;
    [HideInInspector] public float MilliCount;

    [HideInInspector] public string MilliDisplay;
    [HideInInspector] public int BestSecDisplay;
    [HideInInspector] public int BestMinDisplay;
    [HideInInspector] public float BestMiliDisplay;

    
    public GameObject MinDisplay;
    public GameObject SecDisplay;
    public GameObject MilDisplay;
    public GameObject BestMinDisplayObj;
    public GameObject BestSecDisplayObj;
    public GameObject BestMilDisplayObj;
    public CarController2 carController;
    private GameObject gameLogic;
    private bool once = true;
    private float bestLapTime = float.MaxValue; // Initialize best lap time to a high value

    private bool lapCompleted = false;
    private GameManager gameManager;
    private LevelManager levelManager;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned in the GameManager.Instance.");
            return;
        }

        gameLogic = GameObject.Find("GameLogic");
        levelManager = gameLogic.GetComponent<LevelManager>();

        FinishTrig = GameObject.Find("FinishTrigger");

        MinDisplay = GameObject.Find("MinDisplay");
        SecDisplay = GameObject.Find("SecDisplay");
        MilDisplay = GameObject.Find("MilDisplay");
        BestMinDisplayObj = GameObject.Find("BestMinDisplay");

        BestSecDisplayObj = GameObject.Find("BestSecDisplay");
        BestMilDisplayObj = GameObject.Find("BestMilliDisplay");

    }

    private void Start()
    { 
        


    }
    void Update()
    {
        if(once && levelManager.startLevel)
        {
            if (gameManager.playerCarController == null)
            {
                Debug.LogError("Player Car Controller is not assigned in the GameManager.");
                return;
            }
            carController = gameManager.playerCarController;
            once = false;
        }

        if (carController.isEngineRunning && carController != null)
        {
            if (!lapCompleted)
            {
                MilliCount += Time.deltaTime * 10;
                MilliDisplay = MilliCount.ToString("F0");

                MilDisplay.GetComponent<TextMeshProUGUI>().text = "" + MilliDisplay;

                if (MilliCount > 9)
                {
                    MilliCount = 0;
                    SecondCount += 1;
                }

                if (SecondCount <= 9)
                {
                    SecDisplay.GetComponent<TextMeshProUGUI>().text = "0" + SecondCount + ".";
                }
                else
                {
                    SecDisplay.GetComponent<TextMeshProUGUI>().text = "" + SecondCount + ".";
                }

                if (SecondCount >= 60)
                {
                    SecondCount = 0;
                    MinuteCount += 1;
                }

                if (MinuteCount <= 9)
                {
                    MinDisplay.GetComponent<TextMeshProUGUI>().text = "0" + MinuteCount + ".";
                }
                else
                {
                    MinDisplay.GetComponent<TextMeshProUGUI>().text = "" + MinuteCount + ".";
                }
            }
        }
    }

    public void ResetLapTime()
    {
        // Reset lap timer
        MinuteCount = 0;
        SecondCount = 0;
        MilliCount = 0;
        lapCompleted = false; // Reset the lapCompleted flag
    }

    public void UpdateBestLapTime()
    {
        // Check if the current lap time is better than the best lap time
        float currentLapTime = MinuteCount * 60 + SecondCount + MilliCount / 10;
        if (currentLapTime < bestLapTime)
        {
            bestLapTime = currentLapTime;
        }

        // Display the best lap time
        BestMinDisplay = (int)Mathf.Floor(bestLapTime / 60);
        BestSecDisplay = (int)Mathf.Floor(bestLapTime % 60);
        BestMiliDisplay = ((bestLapTime * 10) % 10);

        BestMinDisplayObj.GetComponent<TextMeshProUGUI>().text = BestMinDisplay.ToString("D2") + ".";
        BestSecDisplayObj.GetComponent<TextMeshProUGUI>().text = BestSecDisplay.ToString("D2") + ".";
        BestMilDisplayObj.GetComponent<TextMeshProUGUI>().text = BestMiliDisplay.ToString("F0");
    }
}