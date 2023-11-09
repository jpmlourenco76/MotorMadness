using System.Collections;
using System.Collections.Generic;
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
   
    [SerializeField]
    public List<CarData> cars = new List<CarData>();
   
    [HideInInspector] public bool finished = false;
    [HideInInspector] public List<CarData> finishplacements = new List<CarData>();
    [HideInInspector] public List<CarData> distanceArray;

    private void Awake()
    {
        FinishTrig = GameObject.Find("FinishTrigger");
        Finish = GameObject.Find("Finish");
        FinishPanel = GameObject.Find("FinishPanel");
        finish = Finish.GetComponent<AudioSource>();
        lapsCompleted = FinishTrig.GetComponent<LapsCompleted>();
        lapsCompleted.totallaps = TotalLaps;

    }
    bool ended;
    private void Start()
    {
        ended = true;
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
    }

}
