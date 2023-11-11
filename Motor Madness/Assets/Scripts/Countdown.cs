using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Countdown : MonoBehaviour
{
    private GameObject CountDown;
    private AudioSource Three;
    private AudioSource Two;
    private AudioSource One;
    private AudioSource Go;
    private GameObject LapTimer;
    private GameManager gameManager;

    public CarController2 carController;
    public List<CarController2> AIcarControllers;

    public LevelManager levelManager;

    private void Awake()
    {
        CountDown = GameObject.Find("CountDownUI");
        Three = GameObject.Find("Three").GetComponent<AudioSource>();
        Two = GameObject.Find("Two").GetComponent<AudioSource>();
        One = GameObject.Find("One").GetComponent<AudioSource>();
        Go = GameObject.Find("Go").GetComponent<AudioSource>();
        LapTimer = GameObject.Find("FinishTrigger");
        gameManager = GameManager.Instance;
        levelManager = GetComponent<LevelManager>();



    }


    void Start()
    {
        carController = gameManager.playerCarController;
        AIcarControllers = levelManager.AiControllers;
        StartCoroutine(CountStart());

        
}

    IEnumerator CountStart (){
      
        yield return new WaitForSeconds (0.5f);
        CountDown.GetComponent<TextMeshProUGUI>().text = "3";
        Three.Play ();
        CountDown.SetActive(true);
        yield return new WaitForSeconds (1);
        CountDown.SetActive(false);
        CountDown.GetComponent<TextMeshProUGUI>().text = "2";
        Two.Play ();
        CountDown.SetActive(true);
        yield return new WaitForSeconds (1);
        CountDown.SetActive(false);
        CountDown.GetComponent<TextMeshProUGUI>().text = "1";
        One.Play ();
        CountDown.SetActive(true);
        yield return new WaitForSeconds (1);
        CountDown.SetActive(false);
        Go.Play();
        carController.isEngineRunning = true;

        foreach (CarController2 c in AIcarControllers)
        {
            c.isEngineRunning = true;
        }
        LapTimer.SetActive (true);
        
    }   

}
