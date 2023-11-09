using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int TotalLaps;
    private GameObject FinishTrig;
    public CarController2 carController;
    private LapsCompleted lapsCompleted;
    private GameObject Finish;
    private AudioSource finish;
    private GameObject FinishPanel;
    private Canvas RaceRank;
    private Canvas OverallRank;
    private Canvas MiniMap;
    private Canvas Canvas;

    public GameObject stPlaceDisplay;
    public GameObject ndPlaceDisplay;
    public GameObject rdPlaceDisplay;
    public GameObject thPlaceDisplay;
    private GameObject fthPlaceDisplay;

    public GameObject stPlaceDisplayR;
    public GameObject ndPlaceDisplayR;
    private GameObject rdPlaceDisplayR;
    public GameObject thPlaceDisplayR;
    private GameObject fthPlaceDisplayR;

    public GameObject stPointsR;
    public GameObject ndPointsR;
    public GameObject rdPointsR;
    private GameObject thPointsR;
    private GameObject fthPointsR;






    [SerializeField]
    public List<CarData> cars = new List<CarData>();
   
    [HideInInspector] public bool finished = false;
     public List<CarData> finishplacements = new List<CarData>();
     public List<CarData> Rank = new List<CarData>();
    public List<CarData> distanceArray;

    private void Awake()
    {
        FinishTrig = GameObject.Find("FinishTrigger");
        Finish = GameObject.Find("Finish");
        FinishPanel = GameObject.Find("FinishPanel");
        finish = Finish.GetComponent<AudioSource>();
        lapsCompleted = FinishTrig.GetComponent<LapsCompleted>();
        lapsCompleted.totallaps = TotalLaps;
        RaceRank = GameObject.Find("RaceRank").GetComponent<Canvas>();
        OverallRank = GameObject.Find("OverallRank").GetComponent<Canvas>();
        MiniMap = GameObject.Find("CanvasMiniMap").GetComponent<Canvas>();
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        stPlaceDisplay = GameObject.Find("1stPlaceDisplay");
        ndPlaceDisplay = GameObject.Find("2ndPlaceDisplay");
        rdPlaceDisplay = GameObject.Find("3rdPlaceDisplay");
        thPlaceDisplay = GameObject.Find("4thPlaceDisplay");
        fthPlaceDisplay = GameObject.Find("5thPlaceDisplay");

        stPlaceDisplayR = GameObject.Find("1stPlaceDisplayR");
        ndPlaceDisplayR = GameObject.Find("2ndPlaceDisplayR");
        rdPlaceDisplayR = GameObject.Find("3rdPlaceDisplayR");
        thPlaceDisplayR = GameObject.Find("4thPlaceDisplayR");
        fthPlaceDisplayR = GameObject.Find("5thPlaceDisplayR");

        stPointsR = GameObject.Find("1stPointsR");
        ndPointsR = GameObject.Find("2ndPointsR");
        rdPointsR = GameObject.Find("3rdPointsR");
        thPointsR = GameObject.Find("4thPointsR");
        fthPointsR = GameObject.Find("5thPointsR");


    }
    bool ended;
    private void Start()
    {
        ended = true;
        RaceRank.enabled = false;
        OverallRank.enabled = false;
    }
    

    
    public void EndRace()
    {
        

        if(ended) {
            foreach (CarData car in distanceArray)
            {
                CarData carClone = (CarData)car.Clone();
                finishplacements.Add(carClone);
            }
            ended = false;
            carController.SetDriverType(CarController2.driver.AI); 
        }

        FinishPanel.GetComponent<TextMeshProUGUI>().enabled = true;
        finish.Play();


        Invoke("EnableRaceRank", 3);



    }


    private void EnableRaceRank()
    {
        MiniMap.enabled = false;
        Canvas.enabled = false;


        stPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[0].RacerName;
        ndPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[1].RacerName;
        rdPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[2].RacerName;
        thPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[3].RacerName;
        fthPlaceDisplay.GetComponent<TextMeshProUGUI>().text = finishplacements[4].RacerName;


        RaceRank.enabled = true;

        Invoke("EnableOverallRank", 3);

    }

    private void EnableOverallRank()
    {
        RaceRank.enabled = false;
        int[] prize = { 25, 18, 15, 12, 10 };

        for (int i = 0; i < finishplacements.Count; i++)
        {
            foreach(CarData car in cars)
            {
                if(car.RacerName == finishplacements[i].RacerName)
                {
                    car.points += prize[i];
                }
            }

        }

        foreach (CarData car in cars)
        {
        
            CarData carClone = (CarData)car.Clone();
            Rank.Add(carClone);
            Rank = Rank.OrderByDescending(carClone => carClone.points).ToList();

        }
        stPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[0].RacerName;
        ndPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[1].RacerName;
        rdPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[2].RacerName;
        thPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[3].RacerName;
        fthPlaceDisplayR.GetComponent<TextMeshProUGUI>().text = Rank[4].RacerName;

        stPointsR.GetComponent<TextMeshProUGUI>().text = Rank[0].points.ToString();
        ndPointsR.GetComponent<TextMeshProUGUI>().text = Rank[1].points.ToString();
        rdPointsR.GetComponent<TextMeshProUGUI>().text = Rank[2].points.ToString();
        thPointsR.GetComponent<TextMeshProUGUI>().text = Rank[3].points.ToString();
        fthPointsR.GetComponent<TextMeshProUGUI>().text = Rank[4].points.ToString();





        OverallRank.enabled = true;
    }

}
