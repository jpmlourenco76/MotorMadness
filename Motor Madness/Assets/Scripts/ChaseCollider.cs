using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChaseCollider : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject BustedPanel;



    private void Awake()
    {

        gameManager = GameManager.Instance;
        BustedPanel = GameObject.Find("BustedPanel");



    }
    private void Start()
    {
        BustedPanel.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("chasedcar"))
        {
           

        BustedPanel?.SetActive(true);
        Invoke("GoGarage", 3);



        }
    }


    private void GoGarage()
    {
        gameManager.gameData.characters[0].currentLevel++;
        gameManager.GoGarage();
    }
}
