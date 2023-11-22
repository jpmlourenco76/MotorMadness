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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("chasedcar"))
        {
           

        BustedPanel.GetComponent<TextMeshProUGUI>().enabled = true;
        Invoke("GoGarage", 3);



        }
    }


    private void GoGarage()
    {
        gameManager.gameData.characters[0].currentLevel++;
        gameManager.GoGarage();
    }
}
