using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class MultiplayerCountDown : MonoBehaviourPunCallbacks
{
    private GameObject CountDown;
    private AudioSource Three;
    private AudioSource Two;
    private AudioSource One;
    private AudioSource Go;
    private GameObject LapTimer;
    private GameManager gameManager;

    public CarController2 carController;
    private bool once = true;
    public MultiplayerManager MlevelManager;

    private void Awake()
    {
        CountDown = GameObject.Find("CountDownUI");
        Three = GameObject.Find("Three").GetComponent<AudioSource>();
        Two = GameObject.Find("Two").GetComponent<AudioSource>();
        One = GameObject.Find("One").GetComponent<AudioSource>();
        Go = GameObject.Find("Go").GetComponent<AudioSource>();
        LapTimer = GameObject.Find("FinishTrigger");
        MlevelManager = GetComponent<MultiplayerManager>();
       
    }

    void Start()
    {
        once = true;
        CountDown.SetActive(false);
    }

    private void Update()
    {
        if (MlevelManager.startlevel && once)
        {
            
            once = false;

            CarController2 mycar;
            mycar = MlevelManager.FindLocalPlayer();

            MlevelManager.disableCarControllers(mycar);
            // Call the RPC to start the countdown on all clients
            photonView.RPC("StartCountdown", RpcTarget.All);
        }
    }

    [PunRPC]
    void StartCountdown()
    {
        StartCoroutine(CountStart());
    }

    IEnumerator CountStart()
    {
        yield return new WaitForSeconds(5);
        CountDown.GetComponent<TextMeshProUGUI>().text = "3";
        Three.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<TextMeshProUGUI>().text = "2";
        Two.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<TextMeshProUGUI>().text = "1";
        One.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1);
        CountDown.SetActive(false);
        Go.Play();

        // Enable the engine and LapTimer on the local player
        EnableEngineOnLocalPlayer();
        LapTimer.SetActive(true);
    }


    void EnableEngineOnLocalPlayer()
    {
        CarController2 mycar;
        mycar = MlevelManager.FindLocalPlayer();
        mycar.isEngineRunning = true;

        


    }
}
