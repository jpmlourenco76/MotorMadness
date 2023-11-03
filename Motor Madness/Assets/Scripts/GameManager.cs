using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CarController2 carController;
    public GameObject neeedle;
    private float startPosition = 32f, endPosition = -211f;
    private float desiredPosition;
    public TMPro.TextMeshProUGUI gear;
    public TMPro.TextMeshProUGUI kph;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        kph.text = carController.KPH.ToString("0");
        updateNeedle();
        changeGear();
    }

    public void updateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = carController.EngineRPM / 10000;
        neeedle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));

    }

    public void changeGear()
    {
        gear.text = (carController.CurrentGear != -1) ? (carController.CurrentGear).ToString() : "R";

    }
}
