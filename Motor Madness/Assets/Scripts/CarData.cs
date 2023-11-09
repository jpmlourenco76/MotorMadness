using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarData
{
    public string RacerName;
    public CarController2 controller;
    public float distance;
    public float lap;
    public int points;


    public object Clone()
    {
        return new CarData
        {
            RacerName = RacerName,
            controller = controller,
            distance = distance,
            lap = lap,
            points = points
        };
    }
}


