using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemanagerController : MonoBehaviour
{
    public GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;

        }
    }
    private void Update()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;

        }
    }

    public void GoGarage2()
    {
        gameManager.GoGarage();
    }
}
