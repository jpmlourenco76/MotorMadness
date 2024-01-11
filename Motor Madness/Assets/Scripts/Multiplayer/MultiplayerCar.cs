using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

public class MultiplayerCar : MonoBehaviour
{
    [SerializeField] public PhotonView _photonView;
    [Space]
    [SerializeField] TMP_Text _usernameText;
    [SerializeField] GameObject _usernameCanvas;
    [SerializeField] GameObject _camerasHolder;

    public int carIdentifier = -1;
    private bool camerasON;
    //[SerializeField] GameObject _ownerObj;
    //[Space]
    //[SerializeField] GameObject _needle;
    //[SerializeField] float _vehicleSpeed;

    //float _startPosition = 220f, _endPosition = -41f;
    //float _desiredPosition;

    void Start()
    {
        //TODO: Set Username
        SetUsername();
        camerasON = true;


        if (_photonView.IsMine)
        {
            StartCoroutine(DelayedSetName());

        }
        else
        {
            // This is another player's car
            object identifier;
            if (_photonView.Owner.CustomProperties.TryGetValue("CarIdentifier", out identifier))
            {
                carIdentifier = (int)identifier;
            }

            GameObject Car = gameObject;
            Car.gameObject.name = "Car" + identifier;



        }

    }


    IEnumerator DelayedSetName()
    {
        // Wait for a short delay
        yield return new WaitForSeconds(0.1f);

        // Retrieve the car identifier
        object identifier;
        if (_photonView.Owner.CustomProperties.TryGetValue("CarIdentifier", out identifier))
        {
            carIdentifier = (int)identifier;
        }

        GameObject Car = gameObject;
        Car.gameObject.name = "Car" + identifier;
    }
    void Update()
    {

        if (!_photonView.IsMine)
        {
            if (camerasON) { 
            _camerasHolder.SetActive(false);
            camerasON = false;
            //CarSwitcher.instance.vehicle = gameObject;
            //CarSwitcher.instance.spawnPoints = transform;
            }            
        }
    }

    void SetUsername()
    {
        if (!_photonView.IsMine)
        {
            _usernameCanvas.SetActive(true);
            _usernameText.text = _photonView.Owner.NickName;
        }
        else
        {
            _usernameCanvas.SetActive(false);
        }
    }

    public int GetCarIdentifier()
    {
        return carIdentifier;
    }







}
