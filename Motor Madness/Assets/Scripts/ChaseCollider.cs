using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChaseCollider : MonoBehaviour
{
    private GameManager gameManager;
    public Canvas BustedPanel;



    private void Awake()
    {

        gameManager = GameManager.Instance;
        BustedPanel = GameObject.Find("BustedCanvas").GetComponent<Canvas>();
        


    }
    private void Start()
    {

        if (BustedPanel != null)
        {
            BustedPanel.enabled = false;
        }

    }

    private void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("chasedcar"))
        {


            BustedPanel.enabled = true;
            Invoke("GoGarage", 3);



        }
    }


    private void GoGarage()
    {
        gameManager.gameData.characters[0].currentLevel++;
        gameManager.GoGarage();
    }
}
