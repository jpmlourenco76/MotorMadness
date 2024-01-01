using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using UnityEngine;
using Photon.Pun;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField] GameObject spawnpointOne;
    [SerializeField] GameObject spawnpointTwo;
    [SerializeField] GameObject _playerPrefab;


    private bool isSpawnOneUsed;
    private bool isSpawnTwoUsed;
    private int playerCount;

    void Start()
    {
        //TODO: Spawn Player
        SpawnPlayer();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            }
        }
    }

    void SpawnPlayer()
    {
        if(playerCount <= 1)
        {   
            GameObject _player = PhotonNetwork.Instantiate(_playerPrefab.name, spawnpointOne.transform.position, spawnpointOne.transform.rotation);
            spawnpointOne.SetActive(false);
        }
        else
        {           
            GameObject _player = PhotonNetwork.Instantiate(_playerPrefab.name, spawnpointTwo.transform.position, spawnpointTwo.transform.rotation);
            spawnpointTwo.SetActive(false);
        }
    }
}
