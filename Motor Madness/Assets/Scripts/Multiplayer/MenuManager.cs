using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class MenuManager : MonoBehaviour
{
    public void CloseMenu(GameObject _menu)
    {
        _menu.SetActive(false);
    }

    public void OpenMenu(GameObject _menu)
    {
        _menu.SetActive(true);
    }

    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadPhotonLevel(string _sceneName)
    {
        PhotonNetwork.LoadLevel(_sceneName);
    }
}
