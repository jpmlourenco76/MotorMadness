using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarControlActive : MonoBehaviour
{
    public GameObject CarControl;

    void Start()
    {
        CarControl.GetComponent<CarController2>().enabled = true;
    }
}
