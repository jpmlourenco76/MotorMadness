using UnityEngine;
using Photon.Pun;
using TMPro;

public class MultiplayerCar : MonoBehaviour
{
    [SerializeField] PhotonView _photonView;
    [Space]
    [SerializeField] TMP_Text _usernameText;
    [SerializeField] GameObject _usernameCanvas;
    [SerializeField] GameObject _camerasHolder;

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
}
