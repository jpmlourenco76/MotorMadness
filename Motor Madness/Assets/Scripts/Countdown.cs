using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Countdown : MonoBehaviour
{
    public GameObject CountDown;
    public AudioSource Three;
    public AudioSource Two;
    public AudioSource One;
    public AudioSource Go;
    public GameObject LapTimer;
   
public CarController2 carController;
    public List<CarController2> AIcarControllers;
    void Start()
    {
       
        StartCoroutine (CountStart());

        
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
