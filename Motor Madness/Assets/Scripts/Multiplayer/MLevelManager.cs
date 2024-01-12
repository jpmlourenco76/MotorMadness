using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MLevelManager : MonoBehaviour
{
    private MultiplayerManager multiplayerManager;

    private Canvas RaceRank;
    private Canvas MiniMap;
    private Canvas Canvas;
    private GameObject stPlaceDisplay;
    private GameObject ndPlaceDisplay;
    private GameObject Finish;
    private AudioSource finish;
    private GameObject FinishPanel;
    public List<CarData> finishplacements = new List<CarData>();
    public List<CarData> distanceArray;


    private MLapsCompleted lapsCompleted;
    public int TotalLaps;

    bool once = true;

    private void Awake()
    {
        multiplayerManager = GetComponent<MultiplayerManager>();


        RaceRank = GameObject.Find("RaceRank").GetComponent<Canvas>();
        MiniMap = GameObject.Find("CanvasMiniMap").GetComponent<Canvas>();
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        lapsCompleted = GameObject.Find("FinishTrigger").GetComponent<MLapsCompleted>();
        Finish = GameObject.Find("Finish");
        FinishPanel = GameObject.Find("FinishPanel");
        finish = Finish.GetComponent<AudioSource>();

        lapsCompleted.totallaps = TotalLaps;

        stPlaceDisplay = GameObject.Find("1stPlaceDisplay");
        ndPlaceDisplay = GameObject.Find("2ndPlaceDisplay");




    }

    private void Start()
    {
        RaceRank.enabled = false;
    }



    private void Update()
    {
        if (once && multiplayerManager.racefinish)
        {
            EndRace();
            once = false;
        }
    }
    public void EndRace()
    {

        FinishPanel.GetComponent<TextMeshProUGUI>().enabled = true;
        finish.Play();

        foreach (CarData car in distanceArray)
        {
            CarData carClone = (CarData)car.Clone();
            finishplacements.Add(carClone);
        }


        Invoke("EnableRaceRank", 1.5f);


    }


    private void EnableRaceRank()
    {
        MiniMap.enabled = false;
        Canvas.enabled = false;

        if (RaceRank != null)
        {
            stPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[0].RacerName;
            ndPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[1].RacerName;

            RaceRank.enabled = true;
        }

        Invoke("ReturnToLobby", 3f);
    }

    void ReturnToLobby()
    {

        multiplayerManager.RaceFinish();

    }
}
