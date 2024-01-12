using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXLap2_fire : MonoBehaviour
{
    public GameObject Fire10;
    public GameObject Fire11;
    public GameObject Fire12;
    public GameObject Fire13;
    public GameObject Fire14;
    
    public GameObject Firespot10;
    public GameObject Firespot11;
    public GameObject Firespot12;
    public GameObject Firespot13;
    public GameObject Firespot14;
    
    public AudioClip fireClip; // Renomeei para evitar conflito de nomes
    private AudioSource fireSource; // Renomeei para evitar conflito de nomes

    private bool hasBurned = false;

    void Start()
    {
        fireSource = GetComponent<AudioSource>(); // Corrigi o nome aqui
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && !hasBurned)
        {   
            hasBurned = true;
            ActivateBurns();
        }
    }

    void ActivateBurns()
    {
        GameObject fire10 = Instantiate(Fire10, Firespot10.transform.position, Quaternion.identity); // Corrigi o nome aqui
        GameObject fire11 = Instantiate(Fire11, Firespot11.transform.position, Quaternion.identity); // Corrigi o nome aqui
        GameObject fire12 = Instantiate(Fire12, Firespot12.transform.position, Quaternion.identity); // Corrigi o nome aqui
        GameObject fire13 = Instantiate(Fire13, Firespot13.transform.position, Quaternion.identity); // Corrigi o nome aqui
        GameObject fire14 = Instantiate(Fire14, Firespot14.transform.position, Quaternion.identity); // Corrigi o nome aqui

        if (fireClip != null && fireSource != null)
        {
            fireSource.PlayOneShot(fireClip);
        }
    }
}
