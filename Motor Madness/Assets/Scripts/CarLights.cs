using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarLights : MonoBehaviour
{
    List<LightObject> MainLights = new List<LightObject>();
    List<LightObject> BrakeLights = new List<LightObject>();
    List<LightObject> ReverseLights = new List<LightObject>();

    public CarController2 CarController;
    bool InBrake;
   public bool MainLightsIsOn;
    public event System.Action<CarLightType, bool> OnSetActiveLight;


    private void Awake()
    {
        CarController = GetComponentInParent<CarController2>();
    }

    private void Start()
    {
        var lights = GetComponentsInChildren<LightObject>();
        foreach (var l in lights)
        {
            switch (l.CarLightType)
            {
                case CarLightType.Main:
                    MainLights.Add(l); break;
                case CarLightType.Brake:
                    BrakeLights.Add(l);
                    break;
                case CarLightType.Reverse:
                    ReverseLights.Add(l);
                    break;

            }
        }
        
    }

    private void Update()
    {
        bool carInBrake = CarController != null && CarController.CurrentBrake > 0;
        if (InBrake != carInBrake)
        {
            InBrake = carInBrake;
            SetActiveBrake(InBrake);
        }

        bool reverseactive = CarController != null && CarController.CurrentGear == -1;


        SetActiveReverse(reverseactive);

    }



    public void SwitchMainLights()
    {
        if (MainLights.Count > 0)
        {
            MainLightsIsOn = !MainLightsIsOn;
            SetActiveMainLights(MainLightsIsOn);
        }
    }

    public void SetActiveMainLights(bool value)
    {
        MainLights.ForEach(l => l.Switch(value));

        
        if (OnSetActiveLight != null)
        {
            OnSetActiveLight.Invoke(CarLightType.Main, value);
        }

    }


    public void SetActiveBrake(bool value)
    {
        BrakeLights.ForEach(l => l.Switch(value));


        if (OnSetActiveLight != null)
        {
            OnSetActiveLight.Invoke(CarLightType.Brake, value);
        }

    }

    public void SetActiveReverse(bool value)
    {
        ReverseLights.ForEach(l => l.Switch(value));

        if (OnSetActiveLight != null)
        {
            OnSetActiveLight.Invoke(CarLightType.Reverse, value);
        }

       
    }


    public enum CarLightType
    {
        Main,
        Brake,
        Reverse,
    }
}
