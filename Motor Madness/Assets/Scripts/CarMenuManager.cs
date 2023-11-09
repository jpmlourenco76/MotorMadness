using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMenuManager : MonoBehaviour
{
    public GameObject toRotate;
    public float rotateSpeed;
    public VehicleList listOfVehicles;
    public int vehiclePointer = 0;

    private void Start()
    {
        PlayerPrefs.SetInt("pointer", 0);
        vehiclePointer = PlayerPrefs.GetInt("pointer");

        GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer], Vector3.zero, Quaternion.identity) as GameObject;
        childObject.transform.parent = toRotate.transform;
    }

    private void FixedUpdate()
    {
        toRotate.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void rightButton()
    {
        if(vehiclePointer < listOfVehicles.vehicles.Length - 1 ) 
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            vehiclePointer++;
            PlayerPrefs.SetInt("pointer", vehiclePointer);
            GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer], Vector3.zero, Quaternion.identity) as GameObject;
            childObject.transform.parent = toRotate.transform;
        }
    }
    
    public void leftButton()
    {
        if(vehiclePointer > 0) 
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            vehiclePointer--;
            PlayerPrefs.SetInt("pointer", vehiclePointer);
            GameObject childObject = Instantiate(listOfVehicles.vehicles[vehiclePointer], Vector3.zero, Quaternion.identity) as GameObject;
            childObject.transform.parent = toRotate.transform;
        }
    }
}
