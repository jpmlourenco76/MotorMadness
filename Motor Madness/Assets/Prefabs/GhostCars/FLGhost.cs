using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLGhost : MonoBehaviour
{
    public GameObject FinishTrig;
    public Ghost ghost;

    void OnTriggerEnter()
    {
        if (ghost.isRecord == true)
        {
            ghost.isRecord = false;
        }
        if (ghost.isRecord == false)
        {
            ghost.isRecord = false;
        }

    }
}
