using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject car1;
    public GameObject car2;
    public GameObject car3;
    public GameObject car4;
    public GameObject car5;
    private int randomNumber;
    private GameObject carInstance;

    private bool hasInstantiatedCar = false; 

    private void Start()
    {
        int randomNumber = GetRandomNumber();

        if (randomNumber == 1)
        {
            InstantiatePrefab(car1);
        }
        else if (randomNumber == 2) 
        {
            InstantiatePrefab(car2);
        }
        else if (randomNumber == 3)
        {
            InstantiatePrefab(car3);
        }
        else if (randomNumber == 4)
        {
            InstantiatePrefab(car4);
        }
        else if (randomNumber == 5)
        {
            InstantiatePrefab(car5);
        }            
    }

    private void InstantiatePrefab(GameObject car)
    {        
        carInstance = Instantiate(car);          
    }

    int GetRandomNumber()
    {
        return Random.Range(1, 6);
    }

    public void GoGarage()
    {
        SceneManager.LoadScene("Garage");
    }

    public void GoGarageTraining()
    {
        SceneManager.LoadScene("GarageTraining");
    }

}
