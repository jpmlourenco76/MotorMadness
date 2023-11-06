using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfPointTrigger : MonoBehaviour
{
    public GameObject FinishTrig;
    public GameObject HalfLapTrig;

    void OnTriggerEnter()
    {
        FinishTrig.GetComponent<Collider>().enabled = true;
        HalfLapTrig.SetActive(false);
    }


}
