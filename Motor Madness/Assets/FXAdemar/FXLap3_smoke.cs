using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXLap3_smoke : MonoBehaviour
{
    public GameObject Smoke1;
    public GameObject Smoke2;
    public GameObject Smoke3;
    public GameObject Smoke4;
    public GameObject Smoke5;
    public GameObject Smoke6;
        
    public GameObject Smokespot1;
    public GameObject Smokespot2;
    public GameObject Smokespot3;
    public GameObject Smokespot4;
    public GameObject Smokespot5;
    public GameObject Smokespot6;

    private bool isSmoking = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && !isSmoking)
        {   
            isSmoking = true;
            ActivateSmokes();
        }
    }

    void ActivateSmokes()
    {
        GameObject smoke1 = Instantiate(Smoke1, Smokespot1.transform.position, Quaternion.identity);
        GameObject smoke2 = Instantiate(Smoke2, Smokespot2.transform.position, Quaternion.identity);
        GameObject smoke3 = Instantiate(Smoke3, Smokespot3.transform.position, Quaternion.identity);
        GameObject smoke4 = Instantiate(Smoke4, Smokespot4.transform.position, Quaternion.identity);
        GameObject smoke5 = Instantiate(Smoke5, Smokespot5.transform.position, Quaternion.identity);
        GameObject smoke6 = Instantiate(Smoke6, Smokespot6.transform.position, Quaternion.identity);
    }
}
