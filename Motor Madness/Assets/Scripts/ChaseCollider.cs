using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCollider : MonoBehaviour
{
    private GameManager gameManager;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Car0")
        {
            gameManager.gameData.characters[0].currentLevel++;
            gameManager.GoGarage();
        }
    }
}
