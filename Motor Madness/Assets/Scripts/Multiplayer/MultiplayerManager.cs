
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject spawnpointOne;
    [SerializeField] GameObject spawnpointTwo;
    [SerializeField] GameObject _playerPrefab;

    public GameObject waitingMessage;
    public GameObject disconnectMessage;
    public GameObject Car0, Car1;

    private bool isSpawnOneUsed;
    private bool isSpawnTwoUsed;
    public int playerCount;
    public bool startlevel = false;
    public bool racefinish = false;

    [SerializeField]
    public List<CarData> cars = new List<CarData>();



    void Start()
    {
        disconnectMessage.SetActive(false);
        waitingMessage.SetActive(true);
        isSpawnTwoUsed = false; 
        isSpawnOneUsed = false;
        cars.Clear();

        PhotonNetwork.AddCallbackTarget(this);



        StartCoroutine(WaitForConnectionAndSpawn());
    }

    IEnumerator WaitForConnectionAndSpawn()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);

        CheckAndSpawnPlayers();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }

    void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    void CheckAndSpawnPlayers()
    {
        if (playerCount == 1 && !isSpawnOneUsed)
        {

            Car0 = PhotonNetwork.Instantiate(_playerPrefab.name, spawnpointOne.transform.position, spawnpointOne.transform.rotation);
            Car0.gameObject.name = "Car0";

            SetCarIdentifier(Car0, 0);

            isSpawnOneUsed = true;


        }
        else if (playerCount == 2 && !isSpawnTwoUsed)
        {

             Car1 = PhotonNetwork.Instantiate(_playerPrefab.name, spawnpointTwo.transform.position, spawnpointTwo.transform.rotation);
           Car1.gameObject.name = "Car1";

            SetCarIdentifier(Car1, 1);

            isSpawnTwoUsed = true;

            StartCoroutine(WaitForConnectionAndReset());
        }
    }


    IEnumerator WaitForConnectionAndReset()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);




        photonView.RPC("ResetPlayers", RpcTarget.All);
    }

    [PunRPC]
    void ResetPlayers()
    {


        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        waitingMessage.SetActive(false);

        if(Car0 == null)
        {
            Car0 = GameObject.Find("Car0").gameObject;
        }
        if (Car1 == null)
        {
            Car1 = GameObject.Find("Car1").gameObject;
        }

        CarData car0 = CreateNewCarData(1, Car0.GetComponent<MultiplayerCar>()._photonView.Owner.NickName, Car0);
        CarData car1 = CreateNewCarData(2, Car1.GetComponent<MultiplayerCar>()._photonView.Owner.NickName, Car1);

       

        cars.Add(car0);
        cars.Add(car1);

        foreach (var player in players)
        {
            PhotonView photonView = player.GetPhotonView();

            // Assuming CarController is attached to the player GameObject
            CarController2 carController = player.GetComponent<CarController2>();

            carController.isEngineRunning = false;

            if (photonView.IsMine)
            {
                // Reset position to the respective spawn point
                if (photonView.OwnerActorNr == 1)
                {
                    carController.playerRB.velocity = Vector3.zero;
                    player.transform.position = spawnpointOne.transform.position;
                    player.transform.rotation = spawnpointOne.transform.rotation;
                }
                else if (photonView.OwnerActorNr == 2)
                {
                    carController.playerRB.velocity = Vector3.zero;
                    player.transform.position = spawnpointTwo.transform.position;
                    player.transform.rotation = spawnpointTwo.transform.rotation;
                }

                
                startlevel = true;
            }
        }
    }

    public void SetFinishRace()
    {

        photonView.RPC("RpcSetFinishRace", RpcTarget.All);
    }

    [PunRPC]
    void RpcSetFinishRace()
    {
        racefinish = true;
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

    
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {

        ShowDisconnectCanvas();
    }

    void ShowDisconnectCanvas()
    {
        
        disconnectMessage.SetActive(true);


        Invoke("ReturnToLobby", 3f);
    }


    public void RaceFinish()
    {
        photonView.RPC("ReturnLobby", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ReturnLobby()
    {
        // Load the results scene
        PhotonNetwork.LoadLevel("Lobby");
    }

    void ReturnToLobby()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            PhotonNetwork.CurrentRoom.IsOpen = false; 
            PhotonNetwork.CurrentRoom.IsVisible = false; 
            PhotonNetwork.CurrentRoom.RemovedFromList = true; 

            
        }
        // Leave the room
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        
        SceneManager.LoadScene("Lobby");
    }


    public CarController2 FindLocalPlayer()
    {
        PhotonView[] photonViews = GameObject.FindObjectsOfType<PhotonView>();

        foreach (PhotonView photonView in photonViews)
        {
            if (photonView.IsMine)
            {
                return photonView.GetComponent<CarController2>();
            }
        }

        return null;
    }

    public void disableCarControllers(CarController2 mycar)
    {
        PhotonView[] photonViews = GameObject.FindObjectsOfType<PhotonView>();

        foreach (PhotonView photonView in photonViews)
        {
            if (!photonView.IsMine) {
                
                if (photonView.GetComponent<CarController2>() != mycar)
                {
                    if(photonView.GetComponent<CarController2>() != null)
                    {
                        photonView.GetComponent<CarController2>().enabled = false;
                    }
                    else
                    {
                        Debug.Log("nullphv");
                    }

                   // photonView.gameObject.GetComponentInChildren<CarSFX>().enabled = true;
                    /*   wheelsManager[] wheels = photonView.GetComponentsInChildren<wheelsManager>();

                       foreach (wheelsManager wheel in wheels)
                       {
                           if (wheel != null)
                           {
                               wheel.enabled = false;
                           }
                       }*/
                }

            }
            
        }
    }



    CarData CreateNewCarData(int carID,  string racerName, GameObject car)
    {
        // Create a new CarData instance
        CarData newCar = new CarData();

        // Set the properties of the CarData instance
        newCar.CarID = carID;
        newCar.CarName = " ";
        newCar.RacerName = racerName;
        newCar.CarPrefab = car;
        newCar.distance = 0f;
        newCar.lap = 0;
        newCar.checkpoints = 0;
        newCar.wanderAmount = 0.3f;
        newCar.points = 0;
        newCar.price = 0;
        newCar.material = null;
        newCar.upgradelvls = null;

        // Add the CarData instance to the list
        return newCar;
    }


    void SetCarIdentifier(GameObject car, int identifier)
    {
        PhotonView carPhotonView = car.GetComponent<PhotonView>();

        // Set a custom property to identify the car
        carPhotonView.Owner.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CarIdentifier", identifier } });
    }
}
