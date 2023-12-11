using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CarLights;

public class LightObject : MonoBehaviour
{
    public CarLightType CarLightType;
    public Light LightGO;
  
    public bool LightIsOn { get; private set; }

    private MeshRenderer LightRenderer;


    private void Awake()
    {
        LightRenderer = GetComponent<MeshRenderer>();
    }

    public void Switch(bool value, bool forceSwitch = false)
    {
        if (LightIsOn == value)
        {
            return;
        }

        LightIsOn = value;
        if(LightIsOn)
        {
            LightRenderer.enabled = true;

        }
        else
        {
            LightRenderer.enabled = false;
        }

        if (LightGO)
        {
            LightGO.gameObject.SetActive(LightIsOn);
        }

    }

  
}
