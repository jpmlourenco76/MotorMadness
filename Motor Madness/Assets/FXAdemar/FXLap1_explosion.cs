using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXLap1_explosion : MonoBehaviour
{
    public GameObject Explosion1;
    public GameObject Explosion2;
    public GameObject Explosion3;
    public GameObject Explosion4;
    public GameObject Explosion5;
    public GameObject Explosion6;
    public GameObject Explosion7;

    public GameObject Fire1;
    public GameObject Fire2;
    public GameObject Fire3;
    public GameObject Fire4;
    public GameObject Fire5;
    public GameObject Fire6;
    public GameObject Fire7;
    public GameObject Fire8;
    public GameObject Fire9;

    //public GameObject Scenery_Lap1;
    public GameObject Explosionspot1;
    public GameObject Explosionspot2;
    public GameObject Explosionspot3;
    public GameObject Explosionspot4;
    public GameObject Explosionspot5;
    public GameObject Explosionspot6;
    public GameObject Explosionspot7;
    public GameObject Firespot1;
    public GameObject Firespot2;
    public GameObject Firespot3;
    public GameObject Firespot4;
    public GameObject Firespot5;
    public GameObject Firespot6;
    public GameObject Firespot7;
    public GameObject Firespot8;
    public GameObject Firespot9;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.gameObject.name);
        if (other.CompareTag("Car"))
        {
            ActivateExplosions();
        }
    }

    void ActivateExplosions()
    {
        GameObject explode1 = Instantiate(Explosion1, Explosionspot1.transform.position, Quaternion.identity);
        GameObject explode2 = Instantiate(Explosion2, Explosionspot2.transform.position, Quaternion.identity);
        GameObject explode3 = Instantiate(Explosion3, Explosionspot3.transform.position, Quaternion.identity);
        GameObject explode4 = Instantiate(Explosion4, Explosionspot4.transform.position, Quaternion.identity);
        GameObject explode5 = Instantiate(Explosion5, Explosionspot5.transform.position, Quaternion.identity);
        GameObject explode6 = Instantiate(Explosion6, Explosionspot6.transform.position, Quaternion.identity);
        GameObject explode7 = Instantiate(Explosion7, Explosionspot7.transform.position, Quaternion.identity);
        GameObject fire1 = Instantiate(Fire1, Firespot1.transform.position, Quaternion.identity);
        GameObject fire2 = Instantiate(Fire2, Firespot2.transform.position, Quaternion.identity);
        GameObject fire3 = Instantiate(Fire3, Firespot3.transform.position, Quaternion.identity);
        GameObject fire4 = Instantiate(Fire4, Firespot4.transform.position, Quaternion.identity);
        GameObject fire5 = Instantiate(Fire5, Firespot5.transform.position, Quaternion.identity);
        GameObject fire6 = Instantiate(Fire6, Firespot6.transform.position, Quaternion.identity);
        GameObject fire7 = Instantiate(Fire7, Firespot7.transform.position, Quaternion.identity);
        GameObject fire8 = Instantiate(Fire8, Firespot8.transform.position, Quaternion.identity);
        GameObject fire9 = Instantiate(Fire9, Firespot9.transform.position, Quaternion.identity);

        Debug.Log("Blow!");
    }
}
