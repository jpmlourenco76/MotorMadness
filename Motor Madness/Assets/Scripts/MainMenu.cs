using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoGarage()
    {
        SceneManager.LoadScene("Garage");
    }
    public void GoGarageTraining()
    {
        SceneManager.LoadScene("GarageTraining");
    }

}
