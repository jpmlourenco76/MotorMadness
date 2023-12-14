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
    public GameObject Scenery_Lap1;

    public GameObject Explosion;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.gameObject.name);
        if (other.CompareTag("Car"))
        {
            AtivarExplosoes();
        }
    }

    void AtivarExplosoes()
    {
       
        GameObject explosao1 = Instantiate(Explosion1, Explosion.transform.position, Quaternion.identity);
        GameObject explosao2 = Instantiate(Explosion2, Scenery_Lap1.transform.position, Quaternion.identity);
        GameObject explosao3 = Instantiate(Explosion3, Scenery_Lap1.transform.position, Quaternion.identity);
        GameObject explosao4 = Instantiate(Explosion4, Scenery_Lap1.transform.position, Quaternion.identity);
        GameObject explosao5 = Instantiate(Explosion5, Scenery_Lap1.transform.position, Quaternion.identity);
        GameObject explosao6 = Instantiate(Explosion6, Scenery_Lap1.transform.position, Quaternion.identity);
        GameObject explosao7 = Instantiate(Explosion7, Scenery_Lap1.transform.position, Quaternion.identity);
        
        Debug.Log("Ativar Explosões!");
        
        
    }
}
