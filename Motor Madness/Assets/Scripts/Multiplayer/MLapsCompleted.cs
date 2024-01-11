using Photon.Pun.Demo.PunBasics;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MLapsCompleted : MonoBehaviour
{
    public GameObject CurrentMinDisplay;
    public GameObject CurrentSecDisplay;
    public GameObject CurrentMilDisplay;

    public GameObject BestMinDisplay;
    public GameObject BestSecDisplay;
    public GameObject BestMilDisplay;

    public GameObject LapCounter;
    public GameObject TotalLaps;


    private MultiplayerManager MpManager;
    private MLevelManager levelManager;

    public CarController2 carController;

    private MLapTimeManager lapTimeManager;
    

    private bool once = true;

    public float totallaps;


    private void Awake()
    {
   
     

         MpManager = GameObject.Find("MultiplayerManager").gameObject.GetComponent<MultiplayerManager>();
        levelManager = GameObject.Find("MultiplayerManager").gameObject.GetComponent<MLevelManager>();
        lapTimeManager= GetComponent<MLapTimeManager>();


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

       
    }

    private void Update()
    {
        carController = MpManager.FindLocalPlayer();
        TotalLaps.GetComponent<TextMeshProUGUI>().text = "/ " + totallaps;
        once = false;
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (MpManager.startlevel)
        {

            if (carController.laps <= totallaps)
            {

                LapCounter.GetComponent<TextMeshProUGUI>().text = "" + carController.laps;
            }

            // Check if the required number of laps (e.g., 2 laps) has been completed
            if (carController.laps >= totallaps && carController.human)
            {

                MpManager.SetFinishRace();
            }

            if (carController.laps > 0 && carController.human)
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


}
